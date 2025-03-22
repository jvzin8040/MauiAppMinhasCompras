using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao; // cria a variavel _descrição 

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao 
        { 
            get => _descricao;  // verifica se a descrição é nula
            set 
            {
                if (value == null) // se a descrição for nula, vai ser lançada uma exceção
                { 
                throw new Exception("Descrição não pode ser vazia"); // notifica o usuario que a descrição não pode ser vazia
                }
                else // se a descrição não for nula, a descrição vai ser guardada na variável _descrição
                {
                    _descricao = value; 
                }
            }
        }
        public double Quantidade { get; set; }
        public double Preco { get; set; }
        public double Total { get => Quantidade * Preco; }
    }
}
