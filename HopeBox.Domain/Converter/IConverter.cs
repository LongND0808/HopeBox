namespace HopeBox.Domain.Converter
{
    public interface IConverter<TModel, TDTO>
    {
        TDTO ToDTO(TModel model);
        TModel ToModel(TDTO dto);
        IEnumerable<TDTO> ToListDTO(IEnumerable<TModel> models);
        IEnumerable<TModel> ToListModel(IEnumerable<TDTO> dtos);
    }
}
