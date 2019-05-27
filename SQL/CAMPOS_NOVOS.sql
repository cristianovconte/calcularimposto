ALTER TABLE NotaFiscalItem	
ADD 
BaseIPI DECIMAL(18,5), 
AliquotaIPI DECIMAL(18,5), 
ValorIPI DECIMAL(18,5), 
Desconto INT

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[P_NOTA_FISCAL_ITEM]
(
	@pId int,
    @pIdNotaFiscal int,
    @pCfop varchar(5),
    @pTipoIcms varchar(20),
    @pBaseIcms decimal(18,5),
    @pAliquotaIcms decimal(18,5),
    @pValorIcms decimal(18,5),
    @pNomeProduto varchar(50),
    @pCodigoProduto varchar(20),
	@pBaseIPI decimal(18,5) = 0,
    @pAliquotaIPI decimal(18,5) = 0,
    @pValorIPI decimal(18,5) = 0,
	@pDesconto int = 0
)
AS
BEGIN
	IF (@pId = 0)
	BEGIN 		
		INSERT INTO [dbo].[NotaFiscalItem]
           ([IdNotaFiscal]
           ,[Cfop]
           ,[TipoIcms]
           ,[BaseIcms]
           ,[AliquotaIcms]
           ,[ValorIcms]
           ,[NomeProduto]
           ,[CodigoProduto]
		   ,[BaseIpi]
           ,[AliquotaIpi]
           ,[ValorIpi]
		   ,[Desconto])
		VALUES
           (@pIdNotaFiscal,
			@pCfop,
			@pTipoIcms,
			@pBaseIcms,
			@pAliquotaIcms,
			@pValorIcms,
			@pNomeProduto,
			@pCodigoProduto,
			@pBaseIpi,
			@pAliquotaIpi,
			@pValorIpi,
			@pDesconto)

		SET @pId = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE [dbo].[NotaFiscalItem]
		SET [IdNotaFiscal] = @pIdNotaFiscal
			,[Cfop] = @pCfop
			,[TipoIcms] = @pTipoIcms
			,[BaseIcms] = @pBaseIcms
			,[AliquotaIcms] = @pAliquotaIcms
			,[ValorIcms] = @pValorIcms
			,[NomeProduto] = @pNomeProduto
			,[CodigoProduto] = @pCodigoProduto
			,[BaseIpi] = @pBaseIpi
			,[AliquotaIpi] = @pAliquotaIpi
			,[ValorIpi] = @pValorIpi
			,[Desconto]= @pDesconto
		 WHERE Id = @pId
	END	    
END

GO
