//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZyDMSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DormAdmin
    {
        public int DormAdminID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<bool> Sex { get; set; }
        public string Account { get; set; }
        public string Pwd { get; set; }
        public string Photo { get; set; }
        public string Address { get; set; }
        public Nullable<int> DormitoryID { get; set; }
    
        public virtual Dormitory Dormitory { get; set; }
    }
}
