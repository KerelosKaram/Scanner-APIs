namespace Scanner.Dtos
{
    public class SCColumnsSelectDto
    {
        public int SC_Customer_ID { get; set; }

        public string Group_name { get; set; }

        public string Column_Name { get; set; } = null!;

        public int Type { get; set; }

        public bool Allow_null { get; set; }
        
        public string List { get; set; }

        public int Sort_by { get; set; }

        public int CalcColumn { get; set; }

    }
}