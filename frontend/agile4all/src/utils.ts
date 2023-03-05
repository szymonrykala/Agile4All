
export const dateFormat = {

    short: (date:Date)=>{
        return date.toLocaleTimeString('pl-PL')
    },
    long: (date:Date) => {
        return date.toLocaleDateString('pl-PL')
    }
}

