﻿namespace CustomerManager.Model
{
    public class CustomerRespons
    {
        public CustomerRespons(string id, bool isSuccess)
        {
            Id = id;
            IsSuccess = isSuccess;
        }

        public string Id { get; set; }
        public bool IsSuccess { get; set; }
    }
}
