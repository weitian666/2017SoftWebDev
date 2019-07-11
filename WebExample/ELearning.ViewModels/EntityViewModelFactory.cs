using ELearning.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.ViewModels
{
    public abstract class EntityViewModelFactory<T,TViewModel> where T : class, IEntity,new()  where TViewModel : class,IEntityViewModel, new()
    {

    }
}
