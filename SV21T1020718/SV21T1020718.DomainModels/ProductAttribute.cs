﻿namespace SV21T1020718.DomainModels
{
    public class ProductAttribute
    {
        public long AttributeID { get; set; }
        public int ProductID { get; set; }
        public string AttributeName { get; set; } = "";
        public string AttributeValue { get; set; } = "";
        public int DisplayOrder { get; set; }
    }
}