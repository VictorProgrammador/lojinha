using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionWeb.Domain.Entities
{
    public class PersonBusiness
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }

        [Required(ErrorMessage = "É obrigatório preencher a categoria!")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public string timeAttend { get; set; }
        public string DescriptionBusiness { get; set; }
        public bool Approved { get; set; }
        public bool IsVisible { get; set; }
        public bool IsSubscriber { get; set; }
        public DateTime? LimitSubscription { get; set; }
        public decimal SomaAvaliacoes { get; set; }
        public decimal TotalAvaliacoes { get; set; }
        public decimal mediaAvaliacoes { get; set; }
        public DateTime CreateDate { get; set; }
        public bool HasProducts { get; set; }
        public bool FazerEntregas { get; set; }
        public bool HabilitarCompras { get; set; }
        public bool ProgramaQRCode { get; set; }
        public int QtdCheckinPremio { get; set; }
        public InfraStructure.Response.PagedResult<Product> Products { get; set; }
        public InfraStructure.Response.PagedResult<Raffle> Raffles { get; set; }
        public string TagPrincipal { get; set; }
        public InfraStructure.Response.PagedResult<PersonBusinessArchive> PersonBusinessArchive { get; set; }
    }
}
