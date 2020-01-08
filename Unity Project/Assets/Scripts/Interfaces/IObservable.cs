using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IObservable {
    void AddObserver(IObserver obs);
    void RemoveObserver(IObserver obs);

}
