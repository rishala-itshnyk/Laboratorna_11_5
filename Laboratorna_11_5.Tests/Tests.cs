namespace Laboratorna_11_5.Tests;

[TestFixture]
public class Tests
{
    private string testFileName = "test.bin";
    
    [Test]
    public void WriteAndRead()
    {
        List<Abonent> abonents = new List<Abonent>
        {
            new Abonent { LastName = "Test1", Address = "Address1", Debt = 50.0 },
            new Abonent { LastName = "Test2", Address = "Address2", Debt = 75.0 }
        };

        List<Telephone> phones = new List<Telephone>
        {
            new Telephone { Number = "123", Operator = "Operator1" },
            new Telephone { Number = "456", Operator = "Operator2" }
        };

        Program.Write(testFileName, abonents, phones);
        List<Abonent> readAbonents;
        List<Telephone> readPhones;
        Program.Read(testFileName, out readAbonents, out readPhones);

        CollectionAssert.AreEqual(abonents, readAbonents);
        CollectionAssert.AreEqual(phones, readPhones);
    }
}