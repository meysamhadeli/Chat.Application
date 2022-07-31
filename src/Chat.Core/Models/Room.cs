using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Models
{
    public class Room
    {
        public long Id { get; init; }
        public string RoomName { get; init; }
        public string RoomCreateor { get; init; }
        public string Members { get; init; }
        public DateTime CreatedDate { get; init; }
    }
}
