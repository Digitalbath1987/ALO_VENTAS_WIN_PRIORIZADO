

 DECLARE @NOMBRE_SP VARCHAR(250)
        ,@ID_OBJETO INT



     SET @NOMBRE_SP = #SP

	 
 SELECT @ID_OBJETO = ID 
   FROM SYS.sysobjects WHERE UPPER(LTRIM(RTRIM(NAME))) = UPPER(LTRIM(RTRIM(@NOMBRE_SP)))


/*-------------------------------------------------------------------------------*/
/* SELECCION DE OBJETOS PARAMETROS DE PROCEDMIENTOS ALMACENADOS                  */
/*-------------------------------------------------------------------------------*/
  SELECT OBJ.name       AS SP
        ,OBJ.object_id  AS ID
        ,PAR.name       AS PARAMETRO
        ,TIP.NAME       AS TIPO
        ,PAR.MAX_LENGTH AS LARGO
		,PAR.is_output  AS OUTP
    FROM SYS.objects  OBJ 
   INNER JOIN
         sys.all_parameters PAR 
      ON PAR.object_id = OBJ.object_id
   INNER JOIN
         sys.types   TIP
      ON TIP.SYSTEM_TYPE_ID = PAR.SYSTEM_TYPE_ID
     AND TIP.NAME          != 'sysname'             
   WHERE OBJ.OBJECT_ID      = @ID_OBJETO
   ORDER BY PAR.parameter_id ASC   
    

