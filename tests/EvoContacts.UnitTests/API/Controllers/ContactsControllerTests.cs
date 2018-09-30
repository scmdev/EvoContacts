using EvoContacts.API.Controllers;
using EvoContacts.ApplicationCore.Interfaces;
using EvoContacts.ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EvoContacts.UnitTests.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ContactsControllerTests
    {
        private readonly Mock<IContactService> _mockContactService;
        private readonly Mock<ILogger<ContactsController>> _logger;
        
        private readonly ContactsController _sutContactsController;

        public ContactsControllerTests()
        {
            _mockContactService = new Mock<IContactService>();
            _logger = new Mock<ILogger<ContactsController>>();

            _sutContactsController = new ContactsController(
                _mockContactService.Object, 
                _logger.Object
                );
        }

        #region CONTACTS

        [Fact]
        public async Task ReturnBadRequestObjectResultWhenInvalidModelState()
        {
            _sutContactsController.ModelState.AddModelError("SomeProperty", "A test error");

            var contactCreate = new ContactCreate();
            var result = await _sutContactsController.CreateContact(contactCreate);

            Assert.IsType<BadRequestObjectResult>(result);

            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult?.Value);

            var modelState = objectResult.Value as dynamic;

            Assert.True(modelState?.ContainsKey("SomeProperty"));
        }

        //[Fact]
        //public async Task SaveWhenModelStateValid()
        //{
        //    _mockUnitOfWork.Setup(x => x.SaveAsync()).ReturnsAsync(true);

        //    var contactCreate = new ContactCreate();
        //    await _sut.CreateContact(contactCreate);

        //    _mockContactRepository.Verify(x => x.AddContact(It.IsAny<Contact>()));

        //    _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        //}

        //[Fact]
        //public async Task ContactSavedWhenModelStateValid()
        //{
        //    var contactFromMapper = new Contact
        //    {
        //        Name = "Name",
        //        QuantityPerBox = 10
        //    };
        //    _mockMapper.Setup(x => x.Map<Contact>(It.IsAny<ContactCreate>())).Returns(contactFromMapper);

        //    var contactResourceFromMapper = new ContactResource
        //    {
        //        Id = 5,
        //        Name = "Name",
        //        QuantityPerBox = 10
        //    };
        //    _mockMapper.Setup(x => x.Map<ContactResource>(contactFromMapper)).Returns(contactResourceFromMapper);

        //    Contact contactModelAfterSaved = null;
        //    _mockUnitOfWork.Setup(x => x.SaveAsync()).ReturnsAsync(true)
        //        .Callback(() => contactModelAfterSaved = new Contact
        //        {
        //            Id = 5,
        //            Name = contactFromMapper.Name,
        //            QuantityPerBox = contactFromMapper.QuantityPerBox
        //        });

        //    var contactCreate = new ContactCreate();
        //    var result = await _sut.CreateContact(contactCreate);
        //    var createdAtRouteResult = result as CreatedAtRouteResult;

        //    _mockContactRepository.Verify(x => x.AddContact(It.IsAny<Contact>()), Times.Once);
        //    _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);

        //    Assert.IsType<CreatedAtRouteResult>(result);
        //    Assert.NotNull(createdAtRouteResult);
        //    Assert.IsAssignableFrom<IDictionary<string, object>>(createdAtRouteResult.Value);

        //    var dict = createdAtRouteResult.Value as IDictionary<string, object>;
        //    Assert.NotNull(dict);
        //    Assert.Equal(contactModelAfterSaved.Id, (int)createdAtRouteResult.RouteValues["id"]);
        //    Assert.Equal(contactModelAfterSaved.Name, (string)dict[nameof(contactResourceFromMapper.Name)]);
        //    Assert.Equal(contactModelAfterSaved.QuantityPerBox, (int)dict[nameof(contactResourceFromMapper.QuantityPerBox)]);
        //}

        //[Fact]
        //public async Task ReturnBadRequestWhenContactNull()
        //{
        //    var result = await _sut.CreateContact(null);

        //    Assert.IsType<BadRequestResult>(result);
        //}

        //[Fact]
        //public async Task ThrowExceptionWhenSaveFailed()
        //{
        //    _mockUnitOfWork.Setup(x => x.SaveAsync()).ReturnsAsync(false);

        //    var contactCreate = new ContactCreate();
        //    await Assert.ThrowsAsync<Exception>(async () => await _sut.CreateContact(contactCreate));
        //}

        #endregion

    }
}
