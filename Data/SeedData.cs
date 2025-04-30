using System;
using System.Linq;
using QS.Models;
using QS.Data;
using QS.Enums;  // Ajoute cet import pour les énumérations

namespace QS.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            // Ajouter les services médicaux s'ils n'existent pas déjà
            if (!context.MedicalServices.Any())
            {
                context.MedicalServices.AddRange(
                    new MedicalService { Code = "CONSULT", Name = "Consultation" },
                    new MedicalService { Code = "URG", Name = "Urgences" },
                    new MedicalService { Code = "RADIO", Name = "Radiologie" },
                    new MedicalService { Code = "LAB", Name = "Laboratoire" },
                    new MedicalService { Code = "PHARMA", Name = "Pharmacie" },
                    new MedicalService { Code = "PARAMED", Name = "Paramédical" }
                );
                context.SaveChanges();
            }

            // Ajouter les catégories s'il n'y en a pas
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Categorie { Nom = "Visiteur" },
                    new Categorie { Nom = "Accompagnant" },
                    new Categorie { Nom = "Patient Actif" },
                    new Categorie { Nom = "Patient Retraité" },
                    new Categorie { Nom = "Patient CNAM" },
                    new Categorie { Nom = "Patient PCS" },
                    new Categorie { Nom = "Patient Douane" }
                );
                context.SaveChanges();
            }

            // Ajouter des questions
            if (!context.Questions.Any())
            {
                context.Questions.AddRange(
                    new Question
                    {
                        Text = "Quel est votre âge ?",
                        Type = QuestionType.Numeric,  // Utilisation de l'énumération
                        ServiceType = ServiceType.Consultation,  // Utilisation de l'énumération
                        CreatedDate = DateTime.Now
                    },
                    new Question
                    {
                        Text = "Quel est votre sexe ?",
                        Type = QuestionType.Text,  // Utilisation de l'énumération
                        ServiceType = ServiceType.Urgence,  // Utilisation de l'énumération
                        CreatedDate = DateTime.Now
                    }
                );
                context.SaveChanges();
            }

            // Ajouter des répondants
            if (!context.Repondants.Any())
            {
                context.Repondants.AddRange(
                    new Repondant
                    {
                        Nom = "Anonyme",
                        Categorie = context.Categories.FirstOrDefault(c => c.Nom == "Patient Actif"), 
                        Service = context.MedicalServices.FirstOrDefault(s => s.Code == "CONSULT") 
                    },
                    new Repondant
                    {
                        Nom = "Anonyme",
                        Categorie = context.Categories.FirstOrDefault(c => c.Nom == "Patient Retraité"),
                        Service = context.MedicalServices.FirstOrDefault(s => s.Code == "RADIO")
                    }
                );
                context.SaveChanges();
            }

            // Ajouter des réponses
            if (!context.Reponses.Any())
            {
                context.Reponses.AddRange(
                    new Reponse
                    {
                        Question = context.Questions.FirstOrDefault(q => q.Text == "Quel est votre âge ?"),
                        Repondant = context.Repondants.FirstOrDefault(r => r.Nom == "Anonyme"),
                        NumericValue = 30, // Valeur numérique pour la question sur l'âge
                        TextValue = "J'ai 30 ans" // Valeur textuelle
                    },
                    new Reponse
                    {
                        Question = context.Questions.FirstOrDefault(q => q.Text == "Quel est votre sexe ?"),
                        Repondant = context.Repondants.FirstOrDefault(r => r.Nom == "Anonyme"),
                        TextValue = "Femme"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
