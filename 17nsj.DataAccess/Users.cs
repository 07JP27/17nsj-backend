//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはテンプレートから生成されました。
//
//     このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//     このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace _17nsj.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Users
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsAvailable { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsSysAdmin { get; set; }
        public string Affiliation { get; set; }
    }
}
