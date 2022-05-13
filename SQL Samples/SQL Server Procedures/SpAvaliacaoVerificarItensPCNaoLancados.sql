create Procedure [dbo].[spAvaliacaoVerificarItensPCNaoLancados]
    @in_sq_entidade INT,
	@si_sq_unidnegocio SMALLINT,
	@ti_nr_exercicio_avalrisco TINYINT
 AS


/********************************************************************************************
2008-11-11 
Returns cost sheet items that were not posted as valuation items or were not justified in a contractual exercise
Other information was intentionally omitted
********************************************************************************************/


  BEGIN
	SET NOCOUNT ON

	DECLARE @sd_dt_vigencia_contrato SMALLDATETIME
	DECLARE @in_sq_conteudoprg INT
	DECLARE @si_sq_unidnegocio_emitente SMALLINT
	DECLARE @DataFinalExercicioContratualAtual DATETIME
	DECLARE @sd_dt_confeccao_planilhacusto SMALLDATETIME
	DECLARE @ch_nr_planilhacusto CHAR(15)

	DECLARE @TAvaliacoesRisco TABLE(
		[si_sq_unidnegocio] [SMALLINT] NOT NULL,
		[ch_nr_avalrisco] [char](15) collate database_default NOT NULL
	)

	DECLARE @TItensAvaliacao TABLE(
		[ch_nr_avalrisco] [char](15) collate database_default NULL,
		[in_nr_itemavalrisco] [int] NULL,
		[ch_nr_agente] [char](4)  collate database_default NULL,
		[in_sq_metodoagenterisco] [int] NULL,
		[ti_qt_medicaoquanti_qualitativa] [tinyint] NULL
	)


	DECLARE @TItemCustoAgente TABLE (
		[si_qt_medicoes_itemcustoagente] [int] NULL,
		[ch_nr_agente] [char](4) collate database_default NULL,
		[in_sq_metodoagenterisco] [int] NULL,
		[in_total_grupo] [int] NULL,
		[in_total_disponivel] [int] NULL,
		[vc_ds_localavaliacao_itemcustoagente] [varchar](2000) collate database_default NULL
	)

	/*Initialization of variables*/
    SELECT
		@sd_dt_vigencia_contrato = Contrato.sd_dt_vigencia_contrato,
		@in_sq_conteudoprg = ConteudoPrograma.in_sq_conteudoprg,
		@si_sq_unidnegocio_emitente = ConteudoPrograma.si_sq_unidnegocio_emitente
	FROM
	  dbo.AvaliacaoRisco
		INNER JOIN dbo.ConteudoPrograma ON 
			(
				ConteudoPrograma.in_sq_conteudoprg = AvaliacaoRisco.in_sq_conteudoprg
				AND ConteudoPrograma.si_sq_unidnegocio_emitente = AvaliacaoRisco.si_sq_unidnegocio_emitente
			)
		INNER JOIN dbo.Contrato ON
			(
				Contrato.si_sq_unidnegocio_contrato = ConteudoPrograma.si_sq_unidnegocio_contrato
				AND Contrato.in_sq_contrato = ConteudoPrograma.in_sq_contrato
			)
	WHERE
			AvaliacaoRisco.in_sq_entidade = @in_sq_entidade
			AND AvaliacaoRisco.si_sq_unidnegocio = @si_sq_unidnegocio
			AND AvaliacaoRisco.ti_nr_exercicio_avalrisco = @ti_nr_exercicio_avalrisco


	/*Selection of Risk Assessments that fit the parameters*/
	INSERT @TAvaliacoesRisco
		(
		si_sq_unidnegocio,
		ch_nr_avalrisco
		)
	SELECT
		AvaliacaoRisco.si_sq_unidnegocio,
		AvaliacaoRisco.ch_nr_avalrisco
	FROM
	  dbo.AvaliacaoRisco
	WHERE
			AvaliacaoRisco.in_sq_entidade = @in_sq_entidade
			AND AvaliacaoRisco.si_sq_unidnegocio = @si_sq_unidnegocio
			AND AvaliacaoRisco.ti_nr_exercicio_avalrisco = @ti_nr_exercicio_avalrisco


	/*Selection of Original Assessment Items (leave out added ones)*/
	INSERT @TItensAvaliacao
		(
		ch_nr_avalrisco,
		in_nr_itemavalrisco,
		ch_nr_agente,
		in_sq_metodoagenterisco,
		ti_qt_medicaoquanti_qualitativa
		)
    SELECT
		ItemAvalRisco.ch_nr_avalrisco,
		ItemAvalRisco.in_nr_itemavalrisco,
		ItemAvalRisco.ch_nr_agente,
		ItemAvalRisco.in_sq_metodoagenterisco,
		ItemAvalRiscoQualitativa.ti_qt_medicaoquanti_qualitativa
	FROM
	  @TAvaliacoesRisco AS TAvaliacoesRisco
		INNER JOIN dbo.ItemAvalRisco ON 
			(
				TAvaliacoesRisco.ch_nr_avalrisco = ItemAvalRisco.ch_nr_avalrisco
				AND TAvaliacoesRisco.si_sq_unidnegocio = ItemAvalRisco.si_sq_unidnegocio
			)
		INNER JOIN dbo.ItemAvalRiscoQualitativa ON
			(
				ItemAvalRiscoQualitativa.ch_nr_avalrisco = ItemAvalRisco.ch_nr_avalrisco
				AND ItemAvalRiscoQualitativa.si_sq_unidnegocio = ItemAvalRisco.si_sq_unidnegocio
				AND ItemAvalRiscoQualitativa.in_nr_itemavalrisco = ItemAvalRisco.in_nr_itemavalrisco
			)
	WHERE
		ItemAvalRisco.vc_ds_motinclitem_itemavalrisco IS NULL
		AND ItemAvalRisco.si_sq_justificativamedicao IS NULL

	/*
		Find the cost worksheet (from the Program Content) whose date is the most current and less than or equal to the end date of the assessment exercise in question.
	*/
	/*DataFinal =  DataInicial + 12Meses*@ExercicioAtual + (1Dia*@ExercicioAtual - 1Dia)*/
	SELECT @DataFinalExercicioContratualAtual = DATEADD(mm,12*@ti_nr_exercicio_avalrisco,DATEADD(dd,@ti_nr_exercicio_avalrisco - 1,@sd_dt_vigencia_contrato))

	SELECT
		@sd_dt_confeccao_planilhacusto = MAX(PlanilhaCusto.sd_dt_confeccao_planilhacusto)
	FROM
		dbo.PlanilhaCusto
	WHERE
		PlanilhaCusto.si_sq_unidnegocio_emitente = @si_sq_unidnegocio_emitente
		AND PlanilhaCusto.in_sq_conteudoprg = @in_sq_conteudoprg
		AND PlanilhaCusto.ch_tp_planilhacusto IN ('P','S')
		AND PlanilhaCusto.bi_fg_status_planilhacusto = 1
		AND sd_dt_confeccao_planilhacusto <= @DataFinalExercicioContratualAtual
		AND PlanilhaCusto.ch_cd_etapa_planilhacusto IN ('7','8')
	
	/*Selection of the cost spreadsheet in question*/
	SELECT 
		@ch_nr_planilhacusto = PlanilhaCusto.ch_nr_planilhacusto
	FROM
		dbo.PlanilhaCusto
	WHERE
		PlanilhaCusto.si_sq_unidnegocio_emitente = @si_sq_unidnegocio_emitente
		AND PlanilhaCusto.in_sq_conteudoprg = @in_sq_conteudoprg
		AND PlanilhaCusto.ch_tp_planilhacusto IN ('P','S')
		AND PlanilhaCusto.bi_fg_status_planilhacusto = 1
		AND PlanilhaCusto.ch_cd_etapa_planilhacusto IN ('7','8')
		AND PlanilhaCusto.sd_dt_confeccao_planilhacusto = @sd_dt_confeccao_planilhacusto


	/*Selection of ALL cost items on the worksheet.*/
	INSERT @TItemCustoAgente
		(
		si_qt_medicoes_itemcustoagente,
		ch_nr_agente,
		in_sq_metodoagenterisco,
		in_total_grupo,
		in_total_disponivel,
		vc_ds_localavaliacao_itemcustoagente
		)
	SELECT
		ItemCustoAgente.si_qt_medicoes_itemcustoagente,
		ItemCustoAgente.ch_nr_agente,
		ItemCustoAgente.in_sq_metodoagenterisco,
		0,
		0,
		ItemCustoAgente.vc_ds_localavaliacao_itemcustoagente
	FROM
		dbo.ItemCustoAgente
	WHERE
		ItemCustoAgente.ch_nr_planilhacusto = @ch_nr_planilhacusto

	/*Selection of ALL cost items from related supplemental worksheets.*/
	INSERT @TItemCustoAgente
		(
		si_qt_medicoes_itemcustoagente,
		ch_nr_agente,
		in_sq_metodoagenterisco,
		in_total_grupo,
		in_total_disponivel,
		vc_ds_localavaliacao_itemcustoagente
		)
	SELECT
		ItemCustoAgente.si_qt_medicoes_itemcustoagente,
		ItemCustoAgente.ch_nr_agente,
		ItemCustoAgente.in_sq_metodoagenterisco,
		0,
		0,
		ItemCustoAgente.vc_ds_localavaliacao_itemcustoagente
	FROM
		dbo.PlanilhaCusto
			INNER JOIN dbo.ItemCustoAgente ON (PlanilhaCusto.ch_nr_planilhacusto = ItemCustoAgente.ch_nr_planilhacusto)
	WHERE
		PlanilhaCusto.ch_nr_planilhacusto_principal = @ch_nr_planilhacusto
		AND PlanilhaCusto.ch_tp_planilhacusto = 'C'
		AND PlanilhaCusto.bi_fg_status_planilhacusto = 1
		AND PlanilhaCusto.sd_dt_confeccao_planilhacusto <= @DataFinalExercicioContratualAtual
		AND PlanilhaCusto.ch_cd_etapa_planilhacusto IN ('7','8')

	/*
		Updates ItemCustoAgente records by deducting the value of ItemAvalRisco where there is an equivalent in ItemAvalRisco that has the same "Agente" and the same "Metodo".
	*/	
	UPDATE TItemCustoAgente SET
		in_total_grupo = TotalGrupoTItemCustoAgente.total
	FROM 
		@TItemCustoAgente AS TItemCustoAgente
			INNER JOIN 
			(
				SELECT
					TItemCustoAgente_SUB.ch_nr_agente,
					TItemCustoAgente_SUB.in_sq_metodoagenterisco,
					SUM(ISNULL(TItemCustoAgente_SUB.si_qt_medicoes_itemcustoagente,0)) AS total
				FROM
					@TItemCustoAgente AS TItemCustoAgente_SUB
				GROUP BY
					TItemCustoAgente_SUB.ch_nr_agente,
					TItemCustoAgente_SUB.in_sq_metodoagenterisco
			)AS TotalGrupoTItemCustoAgente ON 
			(
				TotalGrupoTItemCustoAgente.ch_nr_agente = TItemCustoAgente.ch_nr_agente
				AND TotalGrupoTItemCustoAgente.in_sq_metodoagenterisco = TItemCustoAgente.in_sq_metodoagenterisco
			)


	UPDATE TItemCustoAgente SET
		in_total_disponivel = in_total_grupo
	FROM 
		@TItemCustoAgente AS TItemCustoAgente


	UPDATE TItemCustoAgente SET
		in_total_disponivel = in_total_disponivel - TotalTItensAvaliacao.Total
	FROM 
		@TItemCustoAgente AS TItemCustoAgente
			INNER JOIN 
			(
				SELECT
					ch_nr_agente,
					in_sq_metodoagenterisco,
					SUM(ISNULL(ti_qt_medicaoquanti_qualitativa,0)) AS Total
				FROM
					@TItensAvaliacao AS TItensAvaliacao
				GROUP BY
					ch_nr_agente,
					in_sq_metodoagenterisco
			) AS TotalTItensAvaliacao ON 
			(
				TItemCustoAgente.ch_nr_agente = TotalTItensAvaliacao.ch_nr_agente
				AND TItemCustoAgente.in_sq_metodoagenterisco = TotalTItensAvaliacao.in_sq_metodoagenterisco
			)

	/*Return selection*/
	SELECT
		TipoRisco.vc_nm_tiporisco,
		Risco.vc_nm_risco,
		Agente.vc_nm_agente,
		MetodoAgenteRisco.vc_nm_metodoagenterisco,
		TItemCustoAgente.ch_nr_agente,
		TItemCustoAgente.in_sq_metodoagenterisco,
		TItemCustoAgente.in_total_grupo,
		TItemCustoAgente.in_total_disponivel,
		TItemCustoAgente.vc_ds_localavaliacao_itemcustoagente
	FROM 
		@TItemCustoAgente AS TItemCustoAgente
			INNER JOIN AgenteMetodo ON 
			(
				TItemCustoAgente.ch_nr_agente = AgenteMetodo.ch_nr_agente
				AND TItemCustoAgente.in_sq_metodoagenterisco = AgenteMetodo.in_sq_metodoagenterisco
			)
			INNER JOIN Agente ON (AgenteMetodo.ch_nr_agente = Agente.ch_nr_agente)
			INNER JOIN Risco ON (Agente.ch_nr_risco = Risco.ch_nr_risco)
			INNER JOIN TipoRisco ON (Risco.ch_nr_tiporisco = TipoRisco.ch_nr_tiporisco)
			INNER JOIN MetodoAgenteRisco ON (TItemCustoAgente.in_sq_metodoagenterisco = MetodoAgenteRisco.in_sq_metodoagenterisco)
	WHERE
		TItemCustoAgente.in_total_disponivel > 0
	
  END
GO
