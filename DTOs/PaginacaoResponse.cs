namespace API_AGENDA.DTOs
{
    public class PaginacaoResponse<T>
    {
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public List<T> Dados { get; set; } = new();
    }
}
