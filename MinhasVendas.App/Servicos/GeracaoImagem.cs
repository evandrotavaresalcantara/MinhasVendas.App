using MinhasVendas.App.ViewModels;
using System.Drawing;
using System.Drawing.Imaging;

namespace MinhasVendas.App.Servicos
{
    public static class GeracaoImagem
    {
        public static void GerarImagem(string conteudo, string nomeGeradoImagem)
        {
            string diretorio = @"C:\apps\MinhasVendas\MinhasVendas.App\wwwroot\imagensProdutosTmp";
            //string nomeImagem = @"\produto1.png";
            string nomeImagem = $"\\{nomeGeradoImagem}.png";
            string imagemCriadaUpload = diretorio + nomeImagem;


            string FontNome = "Segoe UI";
            int FontTamanho = 15;
            Color FontCor = Color.Black;
            Color CorFundo = Color.AliceBlue;
            int largura = 300;
            int altura = 150;

            var meuBitmap = new Bitmap(largura, altura);
            var meuGrafico = Graphics.FromImage(meuBitmap);

            Color minhaCor = Color.Azure;
            Font minhaFonte = new Font(FontNome, FontTamanho);
            PointF meuPoint = new PointF(5.0F, 5.0F);

            SolidBrush meuPincel1 = new SolidBrush(FontCor);
            SolidBrush meuPincel2 = new SolidBrush(CorFundo);

            meuGrafico.FillRectangle(meuPincel2,0,0,500,500);

            meuGrafico.DrawString(conteudo, minhaFonte, meuPincel1, meuPoint);

            meuBitmap.Save(imagemCriadaUpload, ImageFormat.Png);


        }

        public async static Task<ProdutoViewModel> UploadImagem(string nomeGeradoImagem)
        {
            //string nomeImagem = @"\produto1.png";
            string diretorio = @"C:\apps\MinhasVendas\MinhasVendas.App\wwwroot\imagensProdutosTmp";
            string nomeImagem = $"\\{nomeGeradoImagem}";
            string imagemCriadaUpload = diretorio + nomeImagem;


            using (var fluxoImagemCriada = new FileStream(imagemCriadaUpload, FileMode.Open))
            {

                var imagemRecebida = new MemoryStream();
                await fluxoImagemCriada.CopyToAsync(imagemRecebida);

                var meuFormFile = new FormFile(fluxoImagemCriada, 0, fluxoImagemCriada.Length, "name", nomeImagem.Remove(0, 1));


                ProdutoViewModel produtoViewModel = new ProdutoViewModel();
                produtoViewModel.ImagemUpload = meuFormFile;

                return produtoViewModel;
            }


        }



    }
}
