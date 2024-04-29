using AutoMapper;
using Business;
using Business.Dtos;
using Business.Mappings;
using DataAccess.Data;
using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class PostManagerTests
    {
        private PostManager _postManager;
        private Mock<IBaseModel<Post>> _mockBaseModel;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mockBaseModel = new Mock<IBaseModel<Post>>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>()));
            _postManager = new PostManager(_mockBaseModel.Object, _mapper);
        }

        [Test]
        public void CreatePost_TruncatesBody_WhenExceedsMaxLength()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockBaseModel = new Mock<IBaseModel<Post>>();

            var longText = "En un mundo interconectado y globalizado, la tecnología es vital. Desde la comunicación en redes ...";
            var postDto = new PostDTO { Body = longText };
            var expectedBody = longText.Substring(0, 97) + "...";

            mockMapper.Setup(m => m.Map<Post>(postDto)).Returns(new Post()
            {
                Body = postDto.Body,
                PostId = 1,
            });

            mockBaseModel.Setup(m => m.Create(It.IsAny<Post>())).Returns((Post post) => post);

            var postManager = new PostManager(mockBaseModel.Object, mockMapper.Object);

            // Act
            var createdPost = postManager.CreatePost(postDto);

            // Assert
            Assert.AreEqual(expectedBody, createdPost.Body);
        }


        [Test]
        public void CreatePost_SetsCategory_WhenTypeIsValid()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockBaseModel = new Mock<IBaseModel<Post>>();

            var postDto = new PostDTO { Type = 4, Category = "Category1" }; 

            mockMapper.Setup(m => m.Map<Post>(postDto)).Returns(new Post()
            {
                Type = postDto.Type,
                PostId = 1,
                Category = postDto.Category
            });

            mockBaseModel.Setup(m => m.Create(It.IsAny<Post>())).Returns((Post post) => post);

            var postManager = new PostManager(mockBaseModel.Object, mockMapper.Object);

            // Act
            var createdPost = postManager.CreatePost(postDto);

            // Assert
            Assert.AreEqual("Category1", createdPost.Category); 
        }


    }
}