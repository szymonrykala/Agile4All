import { Input, Option, Select, Sheet, Stack, Typography } from "@mui/joy";
import { useCallback, useLayoutEffect } from "react";
import { ISortItem, useParameterBarContext } from "./Context";
import FilterItem from "./ParameterItem";


interface IParameterBar<T> {
    filters?: (keyof T & string)[],
    sorts?: ISortItem<T>[],
    // search?: string,

    init: {
        sort?: number,
        filter?: number,
        // search?: string,
    }
}


export default function ParameterBar<T>({ sorts, filters, init }: IParameterBar<T>) {
    const { setSort, setFilter, filter, sort } = useParameterBarContext<T>();


    const delayedUpdateFilterValue = useCallback((evt: React.ChangeEvent<HTMLInputElement>) => {
        setFilter({ ...filter, value: String(evt.target.value) } as any);
    }, [
        filter,
        setFilter
    ]);


    useLayoutEffect(() => {
        filters && typeof init.filter === 'number' && setFilter({ key: filters[init.filter], value: '' });
        sorts && typeof init.sort === 'number' && setSort(sorts[init.sort]);
        // search && init.search && setSearch(init.search);
    }, [
        sorts,
        filters,
        init.sort,
        init.filter,
        setSort,
        setFilter
    ]);


    return (
        <Sheet
            sx={{
                padding: 1,
                gap: 3,
                display: 'flex',
                alignItems: 'center',
                flexWrap: 'wrap',
                bgcolor: 'background.componentBg',
                borderRadius: 2
            }}
        >
            {sorts &&
                <Stack direction='row' sx={{ alignItems: 'center' }} spacing={1}>
                    <Typography level='body-md'>
                        Sort by:
                    </Typography>
                    {sorts?.map((_sort, index) =>
                        <FilterItem
                            key={index}
                            onClick={() => setSort(_sort)}
                            clicked={_sort.key === sort?.key}
                            name={_sort.name}
                        />)
                    }
                </Stack>
            }
            {filters &&
                <Stack direction='row' sx={{ alignItems: 'center' }} spacing={1}>
                    <Typography level='body-md'>
                        Filter
                    </Typography>
                    <Select
                        variant="soft"
                        size="sm"
                        value={filter?.key || 'id'}
                        onChange={(evt, value: any) => filter && setFilter({ ...filter, key: value as keyof T & string })}
                    >
                        {
                            filters.map((field, index) =>
                                <Option key={index} value={field}>{field as string}</Option>
                            )
                        }
                    </Select>
                    <Typography level='body-md'>
                        by
                    </Typography>
                    <Input
                        size="sm"
                        value={filter?.value || ''}
                        onChange={delayedUpdateFilterValue}
                        placeholder='filter regexp'
                        sx={{
                            minWidth: '50px'
                        }}
                    />
                </Stack>
            }
        </Sheet>
    )
}