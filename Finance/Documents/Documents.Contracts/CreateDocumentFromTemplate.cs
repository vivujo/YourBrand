﻿namespace YourBrand.Documents.Contracts;

public record CreateDocumentFromTemplate(string TemplateId, DocumentFormat DocumentFormat, string Model);