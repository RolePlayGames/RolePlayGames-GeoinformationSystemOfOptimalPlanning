using LinqToDB.Mapping;

namespace GSOP.Infrastructure.DataAccess.Customers;

[Table(Schema = DBConstants.Schema, Name = TableName)]
public class CustomerPOCO
{
    public const string TableName = "customers";

    [PrimaryKey, Identity]
    required public long ID { get; init; }

    [Column, NotNull]
    required public string Name { get; init; }
}
