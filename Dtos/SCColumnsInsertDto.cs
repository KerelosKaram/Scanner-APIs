namespace Scanner.Dtos
{
    public class SCColumnsInsertDto
    {
        public string? Group_name { get; set; }

        public string Column_Name { get; set; } = null!;

        public int Type { get; set; }
        public bool Allow_null { get; set; }
        public string Insert_User { get; set; }
        public bool Delete_Flag { get; set; }
    }
}