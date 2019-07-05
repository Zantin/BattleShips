using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipsLibrary
{
    public interface ICallbackAble
    {
        void Receive(List<object> result);
    }
}
