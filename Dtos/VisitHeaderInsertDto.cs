using System;

namespace Scanner.Dtos
{
    public class SCVisitHeaderInsertDto
    {
        public int SC_Customer_ID { get; set; }
        public string Scanner_Name { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string Long { get; set; }
        public string lat { get; set; }
        public string Insert_User { get; set; }
    }
}