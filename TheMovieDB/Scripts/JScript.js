// Obtém os filmes, de destaques da semana, configura a lista e exibe na tela. 
var destaqueSemana = function ()
{
    $.get("/Home/ObterDestaqueSemana", function (data)
    {
        console.log(data);
        if (data.result.length > 0) {
            $("#semana").append(listaDestaques(data.result));
        } else {
            $("#semana").append("<p>Nenhum registro encontrado.</p>");
        }
    }).fail(function () {
        $("#semana").append("<p>Nenhum registro encontrado.</p>");
    });
};

// Obtém os filmes, de destaques do mês, configura a lista e exibe na tela. 
var destaqueMes = function (pagina) {
    var url = "/Home/ObterDestaqueMes?pageNumber=" +pagina;
    $.get(url, function (data)
    {
        console.log(data);
        if (data.result.length > 0) {
            $("#mes").append(listaDestaques(data.result));
        } else {
            $("#mes").append("<p>Nenhum registro encontrado.</p>");
        }
    }).fail(function () {
        $("#mes").append("<p>Nenhum registro encontrado.</p>");
    });
};

var listaDestaques = function (lista)
{
    var resultado = "";
    $(lista).each(function(i, e) {
        resultado += criarBoxFilme(e);
    });
    return resultado;
};

// Cria o html que será exibido na tela.
function criarBoxFilme(item)
{
    var resultado = '<div class="media">' +
    '<div class="media-left">' +
    '<a href="/Home/Detalhes/' + item.Id + '" title="' + item.Title + '">' +
    '<img alt="' + item.Title + '" class="media-object" src="https://image.tmdb.org/t/p/w154/' + item.Poster + '" data-holder-rendered="true" />' +
    "</a></div>" +
    '<div class="media-body">' +
    '<h4 class="media-heading">' + item.Title + "</h4>";
        
    if (item.ReleaseDate) {
        var data = eval(item.ReleaseDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
        resultado += "<p>" +data.toLocaleDateString("pt-BR") + "</p>";
    }
    resultado += "<p>" + item.Overview + "</p></div></div>";
    return resultado;
}

// Verifica o posição do Scroll da tela e carrega o restante dos filmes do mês.
function inicializa_scroll_destaqueMes()
{
    $(window).scroll(function(e) 
    {
        if(($(window).scrollTop() + $(window).height() + 20) >= $(document).height()) 
        {
            carregarDestaqueMes($("body").data("pagina"));
        }
    });
}

// Carrega os destaques do mês
function carregarDestaqueMes(pagina)
{
    $("body").data("pagina", pagina + 1);
    destaqueMes($("body").data("pagina"));
}