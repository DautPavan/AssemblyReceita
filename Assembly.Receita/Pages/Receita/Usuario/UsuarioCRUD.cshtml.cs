using Assembly.Database;
using Assembly.Service;
using Assembly.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Assembly.Receita.Pages.CSShared;
using System.Xml;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace Assembly.Receita.Pages.Receita.Usuario
{
    [Authorize(policy: "Master")] // Authenticated
    public class UsuarioPrincipalModel : PageModel
    {
        //inje�ao de depencia
        private readonly IUserRepository _Repository;
        private readonly IUserService _Service;

        public UsuarioPrincipalModel(IUserRepository Repository, IUserService Service)
        {
            _Repository = Repository;
            _Service = Service;
        }

        // ******************************** grupo dados alterar
        // Dados filtrado
        public List<DtosUsuario> dadosFiltro { get; set; } = new List<DtosUsuario>();

        //rota lista - atual
        public string rotaLista { get; set; } = "/Receita/Usuario/UsuarioCRUD";

        //rota lista - cadastro/update/delete/l
        public string rotaCRUD { get; set; } = "/Receita/Usuario/UsuarioCadastrar";

        private string titulo { get; set; } = "Cadastro de Usu�rio";

        
        // Fim  ******************************** grupo dados alterar


        // titulo do cadastro injeta html
        public StringBuilder CabecalhoTitulo { get; set; }

        // campos pesquisa html
        public List<SQLDTOSPesquisa> PesCampo { get; set; } = new();

        // campos pesquisa html
        public List<SQLDTOSPesquisa> PesFinal { get; set; } = new();

        [BindProperty]
        public string TipoFiltro { get; set; }

        [BindProperty]
        public string DadosFiltroPesquisa { get; set; }

        // ********* varaiveis static para pode funcionar todos o paginas recaregadas do lado do servidor
        // dados  a sere pesquisadp 
        public static string DadosPesquisar { get; set; }

        // filtro pesquisa final zero todos itens  1 conforme selecao
        public static int filtroGeral;
        // item pesquisa selecionaodo
        public static int selecao;

        //**********************************************************************************************
        public List<string> listCamposTAB;
        public List<string> descricaoCamposTAB;

        public async Task OnGet()
        {
            //**************************************************** alterar os campos para pesquisa e filtro
            // adciona campos para pesquisa  primeiro
            SQLDTOSPesquisa pesq1= new SQLDTOSPesquisa();
            pesq1.descPesquisa = "Id";
            pesq1.nCampo = "Id";
            pesq1.nType = SQLtypeEnum.type_int;
            pesq1.nValor = DadosFiltroPesquisa;
            pesq1.operacao = SQLoperEnum.p_fim;
            PesCampo.Add(pesq1);

            // adciona campos para pesquisa  segundo
            SQLDTOSPesquisa pesq2 = new SQLDTOSPesquisa();
            pesq2.descPesquisa = "Nome do Usuario";
            pesq2.nCampo = "Name";
            pesq2.nType = SQLtypeEnum.type_string;
            pesq2.nValor = DadosFiltroPesquisa;
            pesq2.operacao = SQLoperEnum.p_fim;
            PesCampo.Add(pesq2);

            // adciona campos para pesquisa  terceiro
            SQLDTOSPesquisa pesq3 = new SQLDTOSPesquisa();
            pesq3.descPesquisa = "User";
            pesq3.nCampo = "UserName";
            pesq3.nType = SQLtypeEnum.type_string;
            pesq3.nValor = DadosFiltroPesquisa;
            pesq3.operacao = SQLoperEnum.p_fim;
            PesCampo.Add(pesq3);

            // adciona campos para pesquisa  primeiro
            SQLDTOSPesquisa pesq4 = new SQLDTOSPesquisa();
            pesq4.descPesquisa = "E-mail";
            pesq4.nCampo = "Email";
            pesq4.nType = SQLtypeEnum.type_string;
            pesq4.nValor = "email";
            pesq4.operacao = SQLoperEnum.p_fim;
            PesCampo.Add(pesq4);

            // FIM  **************************************************** alterar os campos para pesquisa e filtro

            if ( filtroGeral == 0) {
                dadosFiltro = _Service.GetAll();
            }
            else
            {
                PesFinal.Clear();
                var naux = new SQLDTOSPesquisa();

                naux.nCampo = PesCampo[selecao].nCampo;
                naux.operacao = SQLoperEnum.p_fim;
                naux.nValor = DadosPesquisar;
                naux.descPesquisa = PesCampo[selecao].descPesquisa;
                naux.nType = PesCampo[selecao].nType;
                PesFinal.Add(naux);

                dadosFiltro.Clear();
                dadosFiltro = _Service.GetById(PesFinal);
            }
            // titulo 
            CabecalhoTitulo = new CabTituloCRUD().start(titulo);

            // campos a exibir
            List<string> listCamposTAB = new List<string>();
            List<string> descricaoCamposTAB = new List<string>();

            listCamposTAB.Add("Id");
            descricaoCamposTAB.Add("ID");
            listCamposTAB.Add("Name");
            descricaoCamposTAB.Add("Nome Usuario");
            listCamposTAB.Add("UserName");
            descricaoCamposTAB.Add("User Name Acesso");
            listCamposTAB.Add("Email");
            descricaoCamposTAB.Add("Email de Acessoa ao Sistema");
            listCamposTAB.Add("TipoUsuario");
            descricaoCamposTAB.Add("Nivel User");
            listCamposTAB.Add("Ativo");
            descricaoCamposTAB.Add("Ativo?");


            // casting lista dinamica
            var listaDynamic = dadosFiltro.Cast<dynamic>().ToList();

            // viwdata alimentar tela carregada // para o formulario de consulta e lista paginas
            ViewData["PesCampo"] = PesCampo;
            ViewData["rotaLista"] = rotaLista;
            ViewData["PaginaAtual"] = 1;
            ViewData["NumTotaldePag"] = 1;
            //ViewData["btn1Name"] = "Pesquisar";
            //ViewData["btn2Name"] = "Limpar";

            ViewData["dadosFiltro"] = listaDynamic;
            ViewData["rotaCRUD"] = rotaCRUD;
            ViewData["listCamposTAB"] = listCamposTAB;
            ViewData["descricaoCamposTAB"] = descricaoCamposTAB;
            ViewData["NumItensPorPag"] = 1000;    // sem paginacao

        }

        public void OnPost()
        {
            //Console.WriteLine("teste");
            //Console.ReadLine();

        }

        public IActionResult OnPostPesquisaPadrao(string TipoFiltro )
        {
            string botaoClicado = Request.Form["btn9PesquisarFiltro"];

            if (!string.IsNullOrEmpty(botaoClicado))
            {
                if (DadosFiltroPesquisa is not null)
                {
                    filtroGeral = 1;
                    selecao = int.Parse(TipoFiltro);
                    DadosPesquisar = DadosFiltroPesquisa;
                }
            }
            else
            {
                botaoClicado = Request.Form["btn9LimparFiltro"];
                if (!string.IsNullOrEmpty(botaoClicado))
                {
                    filtroGeral = 0;
                }
            }
            return Redirect(rotaLista);
        }

    }
}
