﻿using System;
using System.Collections.Generic;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.User;

namespace eUseControl.BusinessLogic.Interface
{
    public interface IMembershipApi
    {
        void CreateMembership(string name, decimal price, DateTime startDate, DateTime endDate);
        void RemoveMembership(int membershipId);
        Membership GetMembershipById(int membershipId);
        void ApplyDiscount(int membershipId, decimal discountAmount);
        void UpdateMembership(int membershipId, string name, decimal price, DateTime startDate, DateTime endDate);
    }
}
