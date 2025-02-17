﻿import { AddLoading, GetDataGrid, RemoveTramite, getListBit, FolioUpdate } from './Funcionality/ConexxionPeticiones.js'

$(document).ready(() => {

    var t = document.getElementById("frmSearch")
    t.method = "POST"
    t.addEventListener("submit", Submmit)
    AddLoading("listadoInfracciones")
    var Mydata = $("#frmSearch").serialize();
    GetDataGrid(Mydata, FinishGetData)
})


function FinishGetData(view) {
    $("#listadoInfracciones").empty().append(view)
}




const Submmit = (e) => {
    e.preventDefault()
    AddLoading("listadoInfracciones")
    var Mydata = $("#frmSearch").serialize();
    GetDataGrid(Mydata, FinishGetData)
}




window.DataRequestFilter = () => {
    var Mydata = $("#frmSearch").serializeArray();
    var obj = Mydata.reduce((acc, it) => { acc[it.name] = it.value; return acc }, {})
    console.log(obj)
    return obj

}



window.TemplateCortecia = (d) => {
    console.log(d)
    //infraccionCortesia
    console.log("entro")
    if (d.infraccionCortesia) {
        return `<button disabled onclick="ShowCortesia('idInfraccion')" class='w-100 btn'><h6 class='m-0 colorWarning'><i class='icon-edit me-2'></i><b> Cortesía</b></h6></button>`
    } else {
        return `<button disabled onclick="ShowCortesia('idInfraccion')" class='w-100 btn'><h6 class='m-0 colorWarning'><i class='icon-edit me-2'></i><b> Cortesía</b></h6></button>`
    }
}


window.TemplateEditar = (d) => {
    return `<button onclick="ShowUpdate('${d.idInfraccion}' )" class='w-100 btn'><h6 class='m-0 colorPrimary'><i class='icon-edit me-2'></i><b>Ver</b></h6></button>`
}


window.TemplateExportar = (d) => {
    return `<button onclick="CancelTramite(${d.idInfraccion})" class='w-100 btn'><h6 class='m-0 colorPrimary'><b>Eliminar</b></h6></button>`
}


window.TemplateMostrar = (d) => {
    console.log(d)
    console.log(d.idInfraccion)
    return `<form   action="Mostrar">
    <input type="text" value="${d.idInfraccion}" name="id" hidden class="d-none">
    <button  class='w-100 btn'><h6 class='m-0 colorPrimary'><i class='h5 icon-pdf me-2'></i><b>Ver</b></h6></button>
    </form>`
}


window.TemplateFolio = (data) => {
    window.idInfraccion = data.idInfraccion

    return `<div class="d-flex justify-content-between">
    <div>${data.folioInfraccion}</div>
    </div>`
    // <button  class='w-100 btn' onclick="UpdateFolio('${data.idInfraccion}')"><h6 class='m-0 colorPrimary'><i class="icon icon-edit h4 m-0 mt-1 me-2"></i></button>

}


window.UpdateFolio = (id) => {

    $('#modalUpdateFolio').data('updateFolio',id);

    $('#modalUpdateFolio').modal('show');

    //FolioUpdate(id,finishUpdateFolio)

}
window.cerrarModal = () => {
    $('#modalUpdateFolio').modal('hide');

}
window.ActualizarFolio = () => {

    var data = $("#newFolio").val()
    var id = $('#modalUpdateFolio').data('updateFolio')

    FolioUpdate(id,data, finishUpdateFolio)
}


function finishUpdateFolio(d) {
    console.log("hola")
    var grd = $("#GridInf").data("kendoGrid")
    grd.dataSource.read()
    $('#modalUpdateFolio').modal('hide');

}

function finishCancel(d) {


    var grd = $("#GridInf").data("kendoGrid")
    grd.dataSource.read()

}

window.CancelTramite = (d) => {
    console.log("ESTE ES",d)
    $('#modalEliminar').data('id-to-delete', d);

    $('#modalEliminar').modal('show');

    //RemoveTramite(data, finishCancel)

}

var finish = (d) => {
    $('#mostrardetalle').modal('show');

    var text = ""

    d.forEach((w) => {

        text=`${text}
        <tr>
        <td>${w.folio}</td>
        <td>${w.fecha + " " + w.hora}</td>
        <td>${w.nombre}</td>

        <td>${w.operaciondesc}</td>
        <td>${w.desc}</td>
        <td>${w.ip}</td>

        </tr>
        `

    })


    $("#tabdata").empty().append(text)

}


window.ShowUpdate = (d) => {

    var data = { id: d }

    getListBit(data,finish)
}

