using AutoMapper;
using Entities;
using Entities.RequestDTO;
using Entities.Responses;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AccountService : IAccountsService
    {
        private readonly BBBankContext _bbBankContext;
        private readonly IMapper _mapper;
        public AccountService(BBBankContext BBBankContext, IMapper mapper)
        {
            _mapper = mapper;
            _bbBankContext = BBBankContext;
        }
        public async Task OpenAccount(AccountRequestDTO accountRequest)
        {
            // If the user with the same User ID is already in teh system we simply set the userId forign Key of Account with it else 
            // first we create that user and then use it's ID.
            var user = _bbBankContext.Users.FirstOrDefault(x => x.Id == accountRequest.User.Id);
            if (user == null)
            {
                _bbBankContext.Users.Add(accountRequest.User);
                accountRequest.UserId = accountRequest.User.Id;
            }
            else
            {
                accountRequest.UserId = user.Id;
            }

            var account = _mapper.Map<Account>(accountRequest);

            // Setting up ID of new incoming Account to be created.
            account.Id = Guid.NewGuid().ToString();
            // Once User ID forigen key and Account ID Primary Key is set we add the new accoun in Accounts.
            this._bbBankContext.Accounts.Add(account);

        }
    }
}