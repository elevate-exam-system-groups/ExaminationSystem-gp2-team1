using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ExaminationSystem.Domin.Comman
{
    public abstract class Entity
    {
        public Guid Id { get; }

        protected Entity()
        { }

        protected Entity(Guid id)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
        }
    }
}
