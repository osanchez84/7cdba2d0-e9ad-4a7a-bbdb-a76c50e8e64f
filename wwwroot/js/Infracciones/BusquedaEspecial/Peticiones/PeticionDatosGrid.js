
export function GetData(data,Succes,Fail) {

    $.ajax({
        url: './GetDataBusquedaEspecial',
        type: 'POST',
        data: data,
        success: Succes|| function (result) {
            console.log(result);
        },
        error: Fail|| function (result) {
            console.log(result);
        }
    });


}


export function RemoveData(data, Succes, Fail) {

    $.ajax({
        url: './RemoveData',
        type: 'POST',
        data: data,
        success: Succes || function (result) {
            console.log(result);
        },
        error: Fail || function (result) {
            console.log(result);
        }
    });


}