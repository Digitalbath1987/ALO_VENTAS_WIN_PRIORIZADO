<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">

<HTML>
    <HEAD>
        <TITLE>IBR INFOMES</TITLE>
    </HEAD>
    <BODY>

	
		<table style="width: 960px; margin: 5px auto;">
		
			<tr>
			
				<td>
					<img src='cid:imagen' alt="IBR" style="max-width:230px; margin-top: -7px; margin-left: 0px ; height : 60px" ></img>
				</td>
			    <td colspan = "2"></td>
		
			</tr>
			<tr>
			
				<td></td>
			    <td>
					<u><h2> <em> <b>SE INFORMA QUE SU SOLICITUD YA ESTA DISPONIBLE EN DISCADOR</b></em> </h2></u>
				</td>
				<td></td>
			</tr>

  
      

		    <tr style ="margin-top :20px;">

				<td></td>
				<td>


					<div style="float: left;margin-top :20px;">

						<table border="1" >

              <tr>
                <td style ="background: #3498db;color: white;font-weight: bold;padding: 5px;border: 1px solid #ccc;text-align:left;font-size: 10px;">
                  RUTA DE DESCARGA
                </td>
                <td style="text-align:left;color:#6E6E6E;">
                  <xsl:variable name="LINK_DESCARGA"  select='RESULT/Table1/URL' />
                  <a href= "{$LINK_DESCARGA}" >LINK DE DESCARGA</a>

                </td>
              </tr>

						</table>

					</div>          
          
          
          
          
				</td>

				<td></td>
			</tr>
			
  
			
		    <tr style="height:50px;">
			    <td colspan = "3"></td>
		    </tr>			
		    <tr>
			    <td style="text-align:center;color:#6E6E6E;" colspan = "3">
					Departamento de Explotación IBR CHILE. 
				</td>
		    </tr>
		    <tr>
			    <td style="text-align:center;color:#6E6E6E;" colspan = "3">
					<a href="mailto:explotacion@ibrchile.cl?subject=Solicitudes%20a%20explotación">explotacion@ibrchile.cl</a>
				</td>
		    </tr>
		  
		  
		</table>
	
    </BODY>
</HTML>

</xsl:template>
</xsl:stylesheet>