using LinqToDB.Mapping;

namespace GSOP.Infrastructure.DataAccess.Customers;

[Table(Schema = DBConstants.Schema, Name = TableName)]
public class CustomerPOCO
{
    public const string TableName = "customers";

    [PrimaryKey, Identity]
    public long ID { get; init; }

    [Column]
    required public string Name { get; init; }
}
