using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace Core
{
    public interface IFieldModel
    {
        IEnumerable<string> FieldConfigNames();
    }
}
