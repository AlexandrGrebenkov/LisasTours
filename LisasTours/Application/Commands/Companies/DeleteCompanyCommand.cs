﻿using MediatR;

namespace LisasTours.Application.Commands.Companies
{
    public class DeleteCompanyCommand : IRequest
    {
        public int Id { get; set; }

        public DeleteCompanyCommand(int id)
        {
            Id = id;
        }
    }
}
