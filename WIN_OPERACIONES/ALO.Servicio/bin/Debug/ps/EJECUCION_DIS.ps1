clear




<# =================================================================
 DECLARACION DE VARIABLES 
==================================================================#>
 $SERVIDOR                   = #X_P1
 $DB                         = #X_P2
 $USER                       = #X_P3
 $PASS                       = #X_P4
 $ARCHIVO_IN                 = #X_P5
 $ID_FIFO_INTERFAZ           = #X_P6
 $NOMBRE_FILE                = #X_P7
 $V_PID                      = [System.Diagnostics.Process]::GetCurrentProcess().Id





<# =================================================================
 CADENA DE CONECCION 
==================================================================#>
 $CADENA_CONECCION  = "User ID= $USER;
                      Password = $PASS;
                      Initial Catalog = $DB;
                      Data Source= $SERVIDOR;
                      Persist Security Info=True;
                      Pooling=False;
                      Connection Lifetime=1;
                      Application Name=PS_DISCADOR"
                      

<# =================================================================
 SE ACTUALIZA PID 
==================================================================#>
$url ='http://192.9.200.69:9101/DB/RF_DB_GET?json={"R_METODO":{"SISTEMA":"WIN_OPE_V2","SP":"SP_UPDATE_FIFO_INTERFAZ_DIS_X_PROCESO_PID"},"R_PARAM":{"PARAMETROS":{"ID_FIFO_INTERFAZ_DIS":$ID_FIFO_INTERFAZ,"ARCHIVO":"$NOMBRE_FILE","PID":$V_PID}},"R_FILTRO":{"PARAMETROS":{}}}'
$url = $url.Replace('$ID_FIFO_INTERFAZ',$ID_FIFO_INTERFAZ)
$url = $url.Replace('$NOMBRE_FILE',$NOMBRE_FILE)
$url = $url.Replace('$V_PID',$V_PID)
		
		
$request = [System.Net.WebRequest]::Create($url)
$response = $request.GetResponse()
$response.Close()  




  
<# =================================================================
 EJECUCION DE PROCESO
==================================================================#>  
 $SqlConnection = New-Object System.Data.SqlClient.SqlConnection
 $SqlConnection.ConnectionString = $CADENA_CONECCION 
    
    
 Try
 {
   
     
   
  
   
       <# ==========================================================
          SE ABRE CONECCION EN SISTEMA                                       
        ==========================================================#>    
        $SqlConnection.Open()
     
        
        Write-Output "conecto"
        
		$Contador = 1
        
        $reader = [System.IO.File]::OpenText($ARCHIVO_IN)
        while($null -ne ($line = $reader.ReadLine())) {
          
		  
          
          
		  $QUERY = $line;
	
          If (-not ([string]::IsNullOrEmpty($QUERY)))
          {
          
            Try
            {
              $SqlCommand_F = new-object system.data.sqlclient.sqlcommand
              $SqlCommand_F.Connection = $SqlConnection
              $SqlCommand_F.CommandText = $QUERY
              $SqlCommand_F.ExecuteNonQuery()
              Clear-variable -Name "SqlCommand_F"     
            }
            Catch{} 
			
          }  

          
          Write-Output $Contador
          $Contador = $Contador + 1
          
          
          
        }

		<# ==========================================================
          MARCA EL FINAL DE LA EJECUCION                                     
        ==========================================================#>
		$url ='http://192.9.200.69:9101/DB/RF_DB_GET?json={"R_METODO":{"SISTEMA":"WIN_OPE_V2","SP":"SP_UPDATE_FIFO_INTERFAZ_DIS_X_PROCESO"},"R_PARAM":{"PARAMETROS":{"ID_FIFO_INTERFAZ_DIS":$ID_FIFO_INTERFAZ,"ARCHIVO":"$NOMBRE_FILE","EJECUTADO":true}},"R_FILTRO":{"PARAMETROS":{}}}'
		$url = $url.Replace('$ID_FIFO_INTERFAZ',$ID_FIFO_INTERFAZ)
		$url = $url.Replace('$NOMBRE_FILE',$NOMBRE_FILE)
		

		
		$request = [System.Net.WebRequest]::Create($url)
		$response = $request.GetResponse()
		$response.Close()  


		
        
           
   
     
   
   
 }
 Catch
 {
   
     $ErrorMessage = $_.Exception.Message
     $FailedItem = $_.Exception.ItemName
     Write-Output "ERRORES DE PROCESO : $ErrorMessage ITEM : $FailedItem"
     Break
   
 }
 Finally
 {
    
         if ($SqlConnection.State -eq 'Open')
         {
             $SqlConnection.Close();
             Write-Output "CONECCION CERRADA FINALLY"
         }
    
    
 }