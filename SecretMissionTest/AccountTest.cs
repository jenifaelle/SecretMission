﻿using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using SecretMission;

namespace SecretMissionTest
{
    [TestFixture]
    public class AccountTest
    {
        private const int APinNumber = 0;
        private const int AnAccountNumber = 0;
        private const string AFirstName = "John";
        private const string ALastName = "Smith";
        private const string APhoneNumber = "418 666-1234";
        private const string ADateOfBirth = "01/01/2000";
        private const double AnAmount = 20;
        private const double ANegativeAmount = -5;
        private const double ASmallerAmount = 10;
        private readonly List<string> APersonInput;

        private int i;
        private Mock<ILineReaderWriter> consoleMock;
        private ILineReaderWriter console;
        private Account account;

        public AccountTest()
        {
            APersonInput = new List<string>
            {
                AFirstName,
                ALastName,
                APhoneNumber,
                ADateOfBirth
            };
        }

        [SetUp]
        public void Init()
        {
            i = 0;
            consoleMock = new Mock<ILineReaderWriter>();
            consoleMock.Setup(t => t.ReadLine()).Returns(() => APersonInput[i]).Callback(() => i++);
            console = consoleMock.Object;
            account = new Account(AnAccountNumber, APinNumber);
        }

        [Test]
        public void givenValidInformation_whenGenerateAccount_thenTheInformationIsCorrectlyRequested()
        {
            account.GenerateAccount(console);

            consoleMock.Verify(t => t.WriteLine(It.IsAny<string>()), Times.Exactly(4));
        }

        [Test]
        public void givenValidInformation_whenGenerateAccount_thenAccountHasTheRightInformation()
        {
            account.GenerateAccount(console);

            Assert.AreEqual(AFirstName, account.FirstName);
            Assert.AreEqual(ALastName, account.LastName);
            Assert.AreEqual(APhoneNumber, account.PhoneNumber);
            Assert.AreEqual(ADateOfBirth, account.DateOfBirth);
        }

        [Test]
        public void givenAnAmount_whenDeposit_thenAmountIsDeposited()
        {
            account.Deposit(AnAmount);

            Assert.AreEqual(AnAmount, account.Balance);
        }

        [Test]
        public void givenANegativeAmount_whenDeposit_thenNoAmountIsDeposited()
        {
            account.Deposit(ANegativeAmount);

            Assert.AreEqual(0, account.Balance);
        }

        [Test]
        public void givenAnAmountWithSufficientBalance_whenWithdraw_thenAmountIsWithdrawed()
        {
            account.Balance = AnAmount;

            account.Withdraw(AnAmount);

            Assert.AreEqual(0, account.Balance);
        }

        [Test]
        public void givenAnAmountWithInsufficientBalance_whenWithdraw_thenNoAmountIsWithdrawed()
        {
            account.Balance = ASmallerAmount;

            account.Withdraw(AnAmount);

            Assert.AreEqual(ASmallerAmount, account.Balance);
        }
    }
}