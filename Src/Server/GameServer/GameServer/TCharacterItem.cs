//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameServer
{
    using System;
    using System.Collections.Generic;
    
    public partial class TCharacterItem
    {
        public int Id { get; set; }
        public int CharacterID { get; set; }
        public int ItemID { get; set; }
        public int ItemCount { get; set; }
        public int TCharacterID { get; set; }
    
        public virtual TCharacter Owner { get; set; }
    }
}
