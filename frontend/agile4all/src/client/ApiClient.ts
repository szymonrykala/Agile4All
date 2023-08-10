import { useNavigate } from "react-router";
import { UUID } from "../models/common";
import { QueryParams } from "./interface";


export interface ServiceFormData {
    [index: string]: string | number | boolean | undefined | object
}


interface FetchData {
    method: string,
    endpoint: string,
    queryParams?: QueryParams,
    body?: object
    formData?: FormData
}

export interface ResponseData {
    [index: string]: any
}

export interface APIResponse {
    isSuccess: boolean,
    data: ResponseData | null,
    error: string | null
}



export interface StatusMessages {
    [index: number]: string
}


abstract class BaseClient {

    private TOKEN_NAME: string = 'auth_token'; // local storage token variable name
    protected BASE_URL: string = process.env.REACT_APP_API_URL as string

    private HandleRedirect = (path: string) => {
        const navigate = useNavigate();
        navigate(path);
    };

    protected get authToken(): string {
        return window.localStorage.getItem(this.TOKEN_NAME) || '';
    }

    protected set authToken(value: string) {
        window.localStorage.setItem(this.TOKEN_NAME, 'Bearer ' + value);
    }

    public clearAuthToken() {
        window.localStorage.removeItem(this.TOKEN_NAME);
    }

    protected async _fetch(fetchObject: FetchData): Promise<ResponseData> {
        let headers: HeadersInit = {
            'Authorization': this.authToken,
            'User-Agent': process.env.REACT_APP_NAME as string,
        }

        if (!fetchObject?.formData) headers['Content-Type'] = "application/json";

        const response = await fetch(
            this.BASE_URL + fetchObject.endpoint,
            {
                method: fetchObject.method,
                cache: 'no-cache',
                mode: 'cors',
                headers: headers,
                body: fetchObject?.formData ?? JSON.stringify(fetchObject.body)
            }
        );
        let data: ResponseData = {}
        let resp: APIResponse = {
            isSuccess: true,
            data: null,
            error: null
        };

        try {
            resp = await response.json() as APIResponse;

        } catch (error) {
            console.error(error)
        }
        // temp logic
        if (Object.hasOwn(resp, 'isSuccess')) {
            if (resp.isSuccess && resp.data) {
                data = resp.data as ResponseData

            } else if (resp.isSuccess === false && resp.error) {
                console.warn(resp.error)
                throw new Error(resp.error)
            }
        } else {
            data = resp as ResponseData
        }


        if (process.env.NODE_ENV !== 'production') console.debug(data);

        // if user is not authenticated -
        // redirect to let sessionContext to resolve redirections
        if (response.status === 401 && !fetchObject.endpoint.match('/login|register')) {
            this.HandleRedirect('/login');
        }

        if (!response.ok) {
            if (process.env.NODE_ENV !== 'production')
                console.error(response);
            throw new Error(JSON.stringify(response));
        }

        return data as ResponseData;
    }

    protected _get(endpoint: string, queryParams: QueryParams = {}) {
        return this._fetch({
            method: 'GET',
            endpoint: endpoint + '?' + new URLSearchParams(queryParams as any).toString()
        });
    }

    protected _post(endpoint: string, body: object = {}) {
        return this._fetch({
            method: 'POST',
            endpoint: endpoint,
            body: body
        });
    }

    protected _patch(endpoint: string, body: object = {}) {
        if (Object.keys(body).length === 0) return;

        return this._fetch({
            method: 'PATCH',
            endpoint: endpoint,
            body: body
        });
    }

    protected _put(endpoint: string, body: object = {}) {
        return this._fetch({
            method: 'PUT',
            endpoint: endpoint,
            body: body
        });
    }

    protected _delete(endpoint: string) {
        return this._fetch({
            method: 'DELETE',
            endpoint: endpoint
        });
    }
}


export default class ApiClient
    <ICreateData extends object, IUpdateData extends object>
    extends BaseClient {

    public path: string = ''

    getAll(queryParams: QueryParams = {}) {
        return this._get(this.path, queryParams)
    }

    getOne(id: UUID, queryParams: QueryParams = {}) {
        return this._get(`${this.path}/${id}`, queryParams)
    }

    create(body: ICreateData) {
        return this._post(this.path, body)
    }

    update(id: UUID, body: IUpdateData) {
        return this._patch(`${this.path}/${id}`, body)
    }

    delete(id: UUID) {
        return this._delete(`${this.path}/${id}`)
    }
}