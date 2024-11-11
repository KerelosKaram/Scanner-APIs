using System;

namespace Scanner.Dtos
{
    public class SCCustomersSelectDto
    {
        public int SC_Customer_ID { get; set; }
        public string Address { get; set; }
        public float Long { get; set; }
        public float lat { get; set; }
        public int index { get; set; }
        public int New_Index { get; set; }
        public string Elamir_Customer_Code { get; set; }
        public string Customer_Name { get; set; }
        public DateTime LastVisitDate { get; set; }
        public int Color { get; set; }
    }
}