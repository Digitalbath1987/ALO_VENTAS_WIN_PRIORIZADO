clear


<# =================================================================
 PARAMETROS DE ARGUMENTOS 
==================================================================#>
param($PI_SERVIDOR , $PI_DB , $PI_USER, $PI_PASS ,$PI_ARCHIVO_IN ,$PI_ARCHIVO_OUT ,$PI_ID_FIFO_INTERFAZ,$PI_ARCHIVO)




<# =================================================================
 DECLARACION DE VARIABLES 
==================================================================#>
 $SERVIDOR                   = $PI_SERVIDOR
 $DB                         = $PI_DB
 $USER                       = $PI_USER
 $PASS                       = $PI_PASS
 $ARCHIVO_IN                 = $PI_ARCHIVO_IN
 $ARCHIVO_OUT                = $PI_ARCHIVO_OUT
 $ID_FIFO_INTERFAZ           = $PI_ID_FIFO_INTERFAZ
 $ARCHIVO                    = $PI_ARCHIVO
 
 
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
                      Application Name=PS"
                      


  
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
	
          
          $SqlCommand_F = new-object system.data.sqlclient.sqlcommand
          $SqlCommand_F.Connection = $SqlConnection
          $SqlCommand_F.CommandText = $QUERY
          $RD = $SqlCommand_F.ExecuteReader()
          
          
          $Columnas = $RD.FieldCount;
          $Linea =" "
          
          
          
          while ($RD.Read()) 
          {
          
                For ($i=0; $i -le $Columnas- 1; $i++) {
                    $Linea = $Linea + $RD.GetValue($i) + "|"
                }
                
                Add-Content $ARCHIVO_OUT $Linea.Substring(0,$Linea.length-1)
                
                
          }
          $RD.Close()
          Clear-variable -Name "SqlCommand_F"   
           
          
          
          Write-Output $Contador
          $Contador = $Contador + 1
          
          
          
        }
		
		<# ==========================================================
          MARCA EL FINAL DE LA EJECUCION                                     
        ==========================================================#>          
        $QUERY = "EXEC SP_UPDATE_FIFO_INTERFAZ_X_PROCESO $ID_FIFO_INTERFAZ ,$ARCHIVO ,1"
        $SqlCommand_F = new-object system.data.sqlclient.sqlcommand
        $SqlCommand_F.Connection = $SqlConnection
        $SqlCommand_F.CommandText = $QUERY
        $SqlCommand_F.ExecuteNonQuery();
        Clear-variable -Name "SqlCommand_F"        
		
        
           
   
     
   
   
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
