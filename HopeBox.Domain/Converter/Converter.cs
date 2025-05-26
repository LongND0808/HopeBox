using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HopeBox.Domain.Converter
{
    public class Converter<TModel, TDTO> : IConverter<TModel, TDTO>
        where TModel : class, new()
        where TDTO : class, new()
    {
        public TDTO ToDTO(TModel model)
        {
            var dto = new TDTO();
            foreach (var prop in typeof(TModel).GetProperties())
            {
                var dtoProp = typeof(TDTO).GetProperty(prop.Name);
                if (dtoProp != null && dtoProp.CanWrite)
                {
                    dtoProp.SetValue(dto, prop.GetValue(model));
                }
            }
            return dto;
        }

        public TModel ToModel(TDTO dto)
        {
            var model = new TModel();
            foreach (var prop in typeof(TDTO).GetProperties())
            {
                var modelProp = typeof(TModel).GetProperty(prop.Name);
                if (modelProp != null && modelProp.CanWrite)
                {
                    modelProp.SetValue(model, prop.GetValue(dto));
                }
            }
            return model;
        }

        public IEnumerable<TDTO> ToListDTO(IEnumerable<TModel> models)
        {
            return models.Select(ToDTO).ToList();
        }

        public IEnumerable<TModel> ToListModel(IEnumerable<TDTO> dtos)
        {
            return dtos.Select(ToModel).ToList();
        }
    }
}