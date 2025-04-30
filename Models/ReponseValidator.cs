using QS.Enums;  // Assure-toi d'importer l'espace de noms où est définie l'énumération

public class ReponseValidator
{
    // Méthode pour valider le type de question
    public bool ValidateQuestionType(QuestionType questionType)
    {
        // Vérification des types de question valides
        switch (questionType)
        {
            case QuestionType.Scale1To5:
                return true; // L'échelle de 1 à 5 est valide
            case QuestionType.TrueFalse:
                return true; // La question vrai/faux est valide
            default:
                // Si un type de question non pris en charge est rencontré
                return false;
        }
    }

    // Méthode pour valider la réponse associée à un type de question
    public bool ValidateResponse(QuestionType questionType, object response)
    {
        // Validation de la réponse en fonction du type de question
        switch (questionType)
        {
            case QuestionType.Scale1To5:
                return ValidateScale1To5Response(response);
            case QuestionType.TrueFalse:
                return ValidateTrueFalseResponse(response);
            default:
                // Si le type de question n'est pas pris en charge
                return false;
        }
    }

    // Validation des réponses de type échelle 1-5
    private bool ValidateScale1To5Response(object response)
    {
        // Vérifie si la réponse est un entier entre 1 et 5
        if (response is int intResponse)
        {
            return intResponse >= 1 && intResponse <= 5;
        }
        return false;
    }

    // Validation des réponses de type Vrai/Faux
    private bool ValidateTrueFalseResponse(object response)
    {
        // Vérifie si la réponse est de type booléen
        return response is bool;
    }
}
