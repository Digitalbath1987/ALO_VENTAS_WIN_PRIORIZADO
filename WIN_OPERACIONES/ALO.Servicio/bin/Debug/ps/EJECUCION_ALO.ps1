clear




<# =================================================================
 DECLARACION DE VARIABLES 
==================================================================#>
 $SERVIDOR                   = #X_P1
 $DB                         = #X_P2
 $USER                       = #X_P3
 $PASS                       = #X_P4
 $ARCHIVO_IN                 = #X_P5
 $ARCHIVO_OUT                = #X_P6
 $ID_FIFO_INTERFAZ_DIS       = #X_P7
 $ARCHIVO                    = #X_P8
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
                      Application Name=PS_ALO_CONSULTA"
                      

<# =================================================================
 SE ACTUALIZA PID 
==================================================================#>
$url ='http://192.9.200.69:9101/DB/RF_DB_GET?json={"R_METODO":{"SISTEMA":"VENTAS_PRI_PRE","SP":"SP_UPDATE_FIFO_INTERFAZ_DIS_X_PROCESO_PID"},"R_PARAM":{"PARAMETROS":{"ID_FIFO_INTERFAZ_DIS":$ID_FIFO_INTERFAZ_DIS,"ARCHIVO":"$ARCHIVO","PID":$V_PID}},"R_FILTRO":{"PARAMETROS":{}}}'
$url = $url.Replace('$ID_FIFO_INTERFAZ_DIS',$ID_FIFO_INTERFAZ_DIS)
$url = $url.Replace('$ARCHIVO',$ARCHIVO)
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
				  $RD = $SqlCommand_F.ExecuteReader()
              
                  
				  $Columnas = $RD.FieldCount;
				  $Linea =" "
              
              
              
				  while ($RD.Read()) 
				  {
              
						$Linea =""
                    
						For ($i=0; $i -le $Columnas- 1; $i++) {
						    $Valor = $RD.GetValue($i).ToString()

							$Valor = $Valor.Replace('`r','')
							$Valor = $Valor.Replace('`n','')
							$Linea = $Linea + $Valor.trim() + "|"
						}
						$Linea = $Linea.Substring(0,$Linea.length-1) 

						Add-Content $ARCHIVO_OUT $Linea
                    
                    
				  }
				  $RD.Close()
				  Clear-variable -Name "SqlCommand_F" 
			  
			}
            Catch
            {
            
                $ErrorMessage = $_.Exception.Message
                $FailedItem = $_.Exception.ItemName
                Write-Output "ERRORES DE PROCESO : $ErrorMessage ITEM : $FailedItem"
            
            } 			  
			  
			    
          }
          
          
          Write-Output $Contador
          $Contador = $Contador + 1
          
          
          
        }
		$reader.Close()    
		
        
		<# ==========================================================
          MARCA EL FINAL DE LA EJECUCION                                     
        ==========================================================#>
		$url ='http://192.9.200.69:9101/DB/RF_DB_GET?json={"R_METODO":{"SISTEMA":"VENTAS_PRI_PRE","SP":"SP_UPDATE_FIFO_INTERFAZ_DIS_X_PROCESO"},"R_PARAM":{"PARAMETROS":{"ID_FIFO_INTERFAZ_DIS":$ID_FIFO_INTERFAZ_DIS,"ARCHIVO":"$ARCHIVO","EJECUTADO":true}},"R_FILTRO":{"PARAMETROS":{}}}'
		$url = $url.Replace('$ID_FIFO_INTERFAZ_DIS',$ID_FIFO_INTERFAZ_DIS)
		$url = $url.Replace('$ARCHIVO',$ARCHIVO)
		

		
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
