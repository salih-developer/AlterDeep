using System;
using AlterDeep.DBOperations;
using AlterDeep.DBOperations.Model;
using Microsoft.EntityFrameworkCore;

namespace AlterDeep.Test
{
    public class DeepLinkDataFixture : IDisposable
    {
        public EFUnitOfWork _uow = new EFUnitOfWork(new DeepContext(new DbContextOptionsBuilder<DeepContext>()
            .UseInMemoryDatabase(databaseName: "AlterDeep")
            .Options));
        public IRepository<TransactionPage> _transactionPageRepository;
        public IRepository<TransactionPageContents> _transactionPageContentsRepository;
        public IRepository<Content> _contentRepository;
        public IRepository<Flow> _flowRepository;

        public DeepLinkDataFixture()
        {
             _transactionPageRepository = _uow.GetRepository<TransactionPage>();
             _transactionPageContentsRepository = _uow.GetRepository<TransactionPageContents>();
             _contentRepository = _uow.GetRepository<Content>();
             _flowRepository = _uow.GetRepository<Flow>();

            TransactionPage transactionPage = new TransactionPage { Name = "Transaction", FriendlyName = "eft" };
            Content content = new Content { ContentText = "test", Id = 123 };
            TransactionPageContents transactionPageContents = new TransactionPageContents { Content = content, TransactionPage = transactionPage };
            Flow flow = new Flow { Id = 352, Name = "TestEftFlow" };

            _transactionPageRepository.Add(transactionPage);
            _contentRepository.Add(content);
            _transactionPageContentsRepository.Add(transactionPageContents);
            _flowRepository.Add(flow);
            _uow.CommitAsync().GetAwaiter();
        }
        public void Dispose()
        {
            _uow.Dispose();
        }
    }
}
