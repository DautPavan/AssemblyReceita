using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System.Reflection;
using System.Text;

namespace Assembly.Receita.Pages.Receita.Teste
{
    public class testeautoModel : PageModel
    {

        public StringBuilder HtmlContent { get; set; }

        public void OnGet()
        {
            // Inicialize o HtmlContent
            HtmlContent = new StringBuilder();

            // ... seu c�digo para gerar o HTML ...
            //HtmlContent.Append(Cadastra());


            // Adicione o t�tulo
            HtmlContent.Append("<h1>Minha P�gina Din�mica</h1>");

            // Adicione um formul�rio
            //HtmlContent.Append("<form method='post' asp-page-handler='Aprovar'> ");
            HtmlContent.Append("<form method='post'> ");

            // Adicione uma tabela de 5 linhas e 3 colunas
            
            HtmlContent.Append("<table>");
            for (int i = 0; i < 5; i++)
            {
                HtmlContent.Append("<tr>");
                for (int j = 0; j < 3; j++)
                {
                    HtmlContent.Append("<td>Conte�do da c�lula</td>");
                }
                HtmlContent.Append("</tr>");
            }
            HtmlContent.Append("</table>");

            //HtmlContent.Append("<button type='submit' class='btn btn-primary' >Aprovar Publica��o</button>");
            //HtmlContent.Append("<input type='submit' value='Enviar' />");
            HtmlContent.Append("<button type='submit' class='btn btn-primary' name='action' value='Aprovar'>Aprovar Publica��o</button>");
            HtmlContent.Append("<input type='submit' name='action' value='Enviar' />");

            HtmlContent.Append("</form>");


        }

        public IActionResult OnPost()
        {
            var action = Request.Form["action"];

            Console.WriteLine(action);
            Console.ReadLine();



            if (action == "Aprovar")
            {
                return RedirectToPage("/Receita/ReceitaComentario/ComentarioAprovacao");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


    }
}
