//const bootstrap = require("../lib/bootstrap/dist/js/bootstrap");


function EditFormInit() {
    const closeBtnsHtml = document.getElementsByClassName("end");
    const priceInput = document.getElementById("price");
    const modal = document.querySelector(".modal");
    for (const closeBtn of closeBtnsHtml)
    {
        closeBtn.addEventListener("click", () => {
            modal.style.display = "none";
        });
    }
    priceInput.value = priceInput.value.replace(",", ".");
    const editBtn = document.getElementById("editBtn");
    editBtn.addEventListener("click", () => {
        modal.style.display = "none";
        document.getElementById("editForm").submit();
    });
}


function EditCheckFields(e, modelInit) {
    let editModal = document.getElementById("editModal");
    let warningModal = document.getElementById("warningModal");
    const newName = document.getElementById("name").value;
    const newDescription = document.getElementById("description").value;
    const newPrice = document.getElementById("price").value;
    const newStockOffset = document.getElementById("stockOffset").value;
    const newCategories = $('.selectCat').val();
    var sameCat = (newCategories.length == modelInit.selectedCategories.length) && newCategories.every((categoryId, index) => {
        return categoryId === modelInit.selectedCategories[index];
    });
    modelInit.price = modelInit.price.toString().replace(",", ".");
    e.preventDefault();
    $('#editForm').validate();
    if ($('#editForm').valid() === false) return false;
    if (newName == modelInit.name && newDescription == modelInit.description
        && parseFloat(newPrice) == parseFloat(modelInit.price) && (newStockOffset == 0 || newStockOffset == "") && sameCat)
    {
        $(warningModal).modal("toggle");
    }
    else
    {
        $(editModal).modal("toggle");
    }
}



function Askdelete() {
    $(document).on("click", ".delProd", function (e) {
        e.preventDefault();
        let link = $(this).attr("href");
        $(document).on("click", "#deleteBtn", function () {
            window.location.href = link;
        });
    });
}

function ShowDetails(controller, id)
{
    let detailsModal = document.getElementById("detailsModal");
    let detailsFields = document.getElementById("detailsFields");
    document.querySelector("a#editBtn").href = "/" + controller + "/Edit/" + id;
    $.get("/" + controller + "/Details/" + id, function (res) {
        while (detailsFields.firstChild) detailsFields.removeChild(detailsFields.firstChild);
        if (controller == "Product") ProductDetails(res);
        else if (controller == "Client") ClientDetails(res);
        $(detailsModal).modal("toggle");
    });
}

function ProductDetails(res)
{
    let reference_div = document.createElement('div');
    reference_div.classList.add('inlineOptions');
    $(reference_div).append('<p>Référence : </p>' + '<p>' + res.reference + '</p>');
    let name_div = document.createElement('div');
    name_div.classList.add('inlineOptions');
    $(name_div).append('<p>Nom : </p>' + '<p>' + res.name + '</p>');
    let description_div = document.createElement('div');
    description_div.classList.add('inlineOptions');
    $(description_div).append('<p></p>' + '<p>"' + res.description + '"</p>' + '<p></p>');
    let price_div = document.createElement('div');
    price_div.classList.add('inlineOptions');
    $(price_div).append('<p>Prix : </p>' + '<p>' + res.price + '</p>');
    let stock_div = document.createElement('div');
    stock_div.classList.add('inlineOptions');
    $(stock_div).append('<p>Stock : </p>' + '<p>' + res.stock + '</p>');
    let categories_div = document.createElement('div');
    categories_div.classList.add('inlineOptions');
    $(categories_div).append('<p>Catégories(s) : </p>');
    let cat_p = document.createElement('p');
    for (category of res.categories) {
        $(cat_p).append(" " + category);
    }
    categories_div.append(cat_p);
    detailsFields.append(reference_div, name_div, price_div, stock_div, categories_div);
    detailsFields.append(document.createElement('br'));
    detailsFields.append(description_div);
}

function ClientDetails(res)
{
    let firstName_div = document.createElement('div');
    firstName_div.classList.add('inlineOptions');
    $(firstName_div).append('<p>Prénom : </p>' + '<p>' + res.firstName + '</p>');
    let lastName_div = document.createElement('div');
    lastName_div.classList.add('inlineOptions');
    $(lastName_div).append('<p>Nom : </p>' + '<p>' + res.lastName + '</p>');
    let gender_div = document.createElement('div');
    gender_div.classList.add('inlineOptions');
    $(gender_div).append('<p>Genre : </p>' + '<p>' + res.gender + '</p>');
    let reference_div = document.createElement('div');
    reference_div.classList.add('inlineOptions');
    $(reference_div).append('<p>Référence : </p>' + '<p>' + res.reference + '</p>');
    let mail_div = document.createElement('div');
    mail_div.classList.add('inlineOptions');
    $(mail_div).append('<p>Adresse mail : </p>' + '<p>' + res.mail + '</p>');
    let creationDate_div = document.createElement('div');
    creationDate_div.classList.add('inlineOptions');
    $(creationDate_div).append('<p>Date de création : </p>' + '<p>' + res.creationDate + '</p>');
    detailsFields.append(firstName_div, lastName_div, gender_div, reference_div, mail_div, creationDate_div);
}

function SubmitParams(initialSearch) {
    let page = $("#selectPage").val();
    if (initialSearch != $("#searchField :input").val()) {
        page = 1;
    }
    window.location.href = "/product/index?search=" + $("#searchField :input").val() + "&page=" + page;
}

function InitTableSearch(initCurrentPage)
{
    let initialSearch = $("#searchField :input").val();
    $('#selectPage').val(initCurrentPage);
    $(document).on("click", "#searchField :button", (e) => {
        e.preventDefault();
        SubmitParams(initialSearch);
    });
    $(document).on("change", "#selectPage", (e) => {
        e.preventDefault();
        SubmitParams(initialSearch);
    });
}

function OCAutoComplete() {
    $('#clientInput').on("input", () => {
        $('#hiddenInput').val($('#clientsList [value="' + $('#clientInput').val() + '"]').data("value"));
        console.log($('#hiddenInput').val());
        $.get("/Client/GetFromName?name=" + $('#clientInput').val(), function (res) {
            let clientsList = document.getElementById("clientsList");
            while (clientsList.firstChild) clientsList.removeChild(clientsList.firstChild);
            if (Object.keys(res).length != 0) {
                for (let client of res) {
                    let clientOption = document.createElement('option');
                    clientOption.value = client.firstName + " " + client.lastName + " (" + client.reference + ")";
                    $(clientOption).data("value", client.id);
                    clientsList.append(clientOption);
                }
            }
        });
    });
}

function InitOLModal() {
    $(document).on("click", ".addOLine", function (e) {
        e.preventDefault();
        AddOL();
    });
    $('#addOLBtn').on("click", () => {
        if (OLCheckStock() && OLCheckBag()) {
            $('#addOLModal').modal("toggle");
            OLSubmit();
        }
    });
    OLnoElement();
}

function AddOL() {
    let productDetails = document.getElementById('productDetails');
    while (productDetails.firstChild) productDetails.removeChild(productDetails.firstChild);
    let warningOL = document.getElementById('warningOL');
    warningOL.innerText = "";
    $('#productInput').val('');
    ProdAutoComplete();
    $('#addOLModal').modal("toggle");
}

function OLSubmit() {
    let OLTable = document.getElementById('OLTable');
    let OLSelect = document.getElementById('hiddenOLSelect');
    let OLOption = document.createElement('option');
    let productCarac = JSON.parse(document.getElementById('productCarac').innerText);
    OLOption.selected = true;
    OLOption.setAttribute("name", $('#productReference').text());
    OLOption.value = "{'quantity' : " + $('#inputBag').val() + ", 'productId' : " + productCarac.id + "}";
    OLSelect.append(OLOption);
    let row = document.createElement('tr');
    let prodCol = document.createElement('td');
    prodCol.innerText = "(" + $('#productReference').text() + ") " + productCarac.name;
    let quantCol = document.createElement('td');
    quantCol.innerText = $('#inputBag').val();
    let actionCol = OLPrintActionsCol(row, OLOption);
    row.append(prodCol, quantCol, actionCol);
    OLTable.append(row);
    OLnoElement();
}

function OLPrintActionsCol(row, OLOption) {
    let actionCol = document.createElement('td');
    actionCol.classList.add('text-center');
    actionCol.innerText = "";
    let actionDelete = document.createElement('a');
    actionDelete.href = "#";
    actionDelete.classList.add('btn', 'btn-danger');
    actionDelete.innerText = " X ";
    actionDelete.addEventListener("click", () => {
        OLTable.removeChild(row);
        let OLSelect = document.getElementById('hiddenOLSelect');
        OLSelect.removeChild(OLOption);
        OLnoElement();
    });
    actionCol.append(actionDelete);
    return actionCol;
}

function OLnoElement() {
    let OLTable = document.getElementById('OLTable');
    let rowsNbr = OLTable.childElementCount;
    if (rowsNbr == 0) {
        let defaultRow = document.createElement('tr');
        let defaultCol = document.createElement('td');
        defaultCol.colSpan = "3";
        defaultCol.classList.add("text-center");
        defaultCol.innerText = "Aucun produit dans le panier pour le moment";
        defaultRow.setAttribute("id", "defaultRow");
        defaultRow.append(defaultCol);
        OLTable.append(defaultRow);
    }
    else {
        let defaultRow = document.getElementById('defaultRow');
        if (defaultRow != null) OLTable.removeChild(defaultRow);
    }
}

function OLCheckStock() {
    if ($('#inputBag').val() > 0 && $('#inputBag').val() <= parseInt($('#productStock').text())) {
        return true;
    }
    else return false;
}

function OLCheckBag(reference) {
    let OLSelect = document.getElementById('hiddenOLSelect');
    for (let option of OLSelect) {
        if (option.getAttribute("name") == $('#productReference').text()) {
            $('#warningOL').text("Ce produit est déjà dans votre panier");
            return false;
        }
    }
    return true;
}

function ProdAutoComplete() {
    $('#productInput').on("input", () => {
        $('#hiddenOLInput').val($('#productsList [value="' + $('#productInput').val() + '"]').data("value"));
        if ($('#hiddenOLInput').val().length > 0) {
            $.get("/Product/GetById?id=" + $('#hiddenOLInput').val(), function (product) {
                OLForm(product);
            });
        }
        else
        {
            $.get("/Product/GetFromName?name=" + $('#productInput').val(), function (res) {
                let productsList = document.getElementById("productsList");
                while (productsList.firstChild) productsList.removeChild(productsList.firstChild);
                if (Object.keys(res).length != 0) {
                    for (let product of res) {
                        let productOption = document.createElement('option');
                        productOption.value = product.name + " (" + product.reference + ")";
                        $(productOption).data("value", product.id);
                        productsList.append(productOption);
                    }
                }
            });
        }
    });
}

function OLForm(product)
{
    $('#warningOL').text("");
    let productDetails = document.getElementById('productDetails');
    while (productDetails.firstChild) productDetails.removeChild(productDetails.firstChild);
    let productDesc = document.createElement('div');
    productDesc.classList.add('inlineOptions', 'text-tan');
    $(productDesc).append('<p></p>' + '<p>"' + product.description + '"</p>' + '<p></p>');
    let prodIBody = document.createElement('div');
    prodIBody.classList.add('inlineOptions');
    let prodIBodyLeft = OLCreateLeftBody(product);
    let prodIBodyRight = OLCreateRightBody(product);
    prodIBody.append(prodIBodyLeft, prodIBodyRight);
    productDetails.append(productDesc, prodIBody);
}

function OLCreateLeftBody(product) {
    let prodIBodySub1 = document.createElement('div');
    let refDiv = document.createElement('div');
    refDiv.innerText = 'Référence : ';
    let reference = document.createElement('span');
    reference.setAttribute("id", "productReference");
    reference.innerText = product.reference;
    refDiv.append(reference);
    let priceDiv = document.createElement('div');
    priceDiv.setAttribute("id", "productPrice");
    $(priceDiv).append('<span>Prix : ' + product.price + '</span>');
    let stockDiv = document.createElement('span');
    stockDiv.innerText = 'Stock : ';
    let stock = document.createElement('span');
    stock.setAttribute("id", "productStock");
    stock.innerText = product.stock;
    stockDiv.append(stock);
    $(prodIBodySub1).append(refDiv, priceDiv, stockDiv);
    return prodIBodySub1;
}

function OLCreateRightBody(product) {
    let prodIBodySub2 = document.createElement('div');
    prodIBodySub2.classList.add('flexColAr');
    let prodCaracDiv = document.createElement('div');
    prodCaracDiv.style.display = "none";
    let productCarac = document.createElement('span');
    productCarac.setAttribute("id", "productCarac");
    productCarac.style.display = "none";
    productCarac.innerText = '{"id" : ' + product.id + ', "name" : "' + product.name + '"}';
    prodCaracDiv.append(productCarac);
    let formBag = document.createElement('form');
    $(formBag).append('<p><label>Quantité : </label> <input id="inputBag" size="4" type="number" value="0" name="bagQuantity"></button></p>');
    let totalOL = document.createElement('div');
    totalOL.setAttribute("id", "totalOL")
    $(totalOL).append('<p> Prix total : ' + 0 + '</p>');
    $(formBag).on("input", () => {
        OLTotal(product.price);
    });
    prodIBodySub2.append(prodCaracDiv, formBag, totalOL);
    return prodIBodySub2;
}

function OLTotal(price) {
    let totalOL = document.getElementById('totalOL');
    while (totalOL.firstChild) totalOL.removeChild(totalOL.firstChild);
    if (OLCheckStock()) {
        $('#warningOL').text("");
        $(totalOL).append('<p> Prix total : ' + ($('#inputBag').val() * price).toFixed(2) + '</p>');
    }
    else {
        $('#inputBag').val(0);
        $(totalOL).append('<p> Prix total : ' + 0 + '</p>');
        $('#warningOL').text("Veuillez entrer une quantité correcte!");
    }
}

function OrderEditInit() {
    let initRows = document.querySelectorAll('#OLTable > tr');
    initRows.forEach(function (OLRow, index) {
        if (OLRow.getAttribute('id') != 'defaultRow') {
            let actionCol = OLRow.lastElementChild;
            let actionDelete = document.createElement('a');
            actionDelete.href = "#";
            actionDelete.classList.add('btn', 'btn-danger');
            actionDelete.innerText = " X ";
            actionDelete.addEventListener("click", () => {
                OLTable.removeChild(OLRow);
                let OLSelect = document.getElementById('hiddenOLSelect');
                let OLOption = document.querySelector('#hiddenOLSelect > option[name*="' + OLRow.getAttribute('name') + '"]');
                OLSelect.removeChild(OLOption);
                OLnoElement();
            });
            actionCol.append(actionDelete);
        }
    });
}