<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="html" encoding="utf-8" indent="yes"/>

  
  <xsl:template match="/Detalles">


    <div class="table-responsive">
      <table>
        <tr>
          <td>
            <h2>INFORMES DE APLICACIÓN</h2>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="Mensaje"/>
          </td>
        </tr>
        <tr>
          <td>
            <IMG id = "IMG_MostrarError" SRC="/IMAGES/ques.png" ALT="Mostrar Detalles" onClick="MostrarThrow();"></IMG>
            <IMG id = "IMG_OcultarError" SRC="/IMAGES/OK.png" ALT="Ocultar Detalles" onClick="OcultarThrow();"  style ="display: none;" ></IMG>
          </td>
        </tr>
        <tr>
          <td>

            <div id = "OcultoThrowError" style ="display: none;" >
              <table>
                <tr>
                  <td>
                    Nombre Metodo
                  </td>
                  <td>:</td>
                  <td>
                    <xsl:value-of select="NombreMetodo"/>
                    <BR/>
                  </td>
                </tr>
                <tr>
                  <td>
                    Clase
                  </td>
                  <td>:</td>
                  <td>
                    <xsl:value-of select="Clase"/>
                    <BR/>
                  </td>
                </tr>
                <tr>
                  <td>
                    NameSpace
                  </td>
                  <td>:</td>
                  <td>
                    <xsl:value-of select="NameSpace"/>
                    <BR/>
                  </td>
                </tr>
                <tr>
                  <td>
                    Mensaje
                  </td>
                  <td>:</td>
                  <td>
                    <xsl:value-of select="Mensaje"/>
                    <BR/>
                  </td>
                </tr>
                <tr>
                  <td>
                    Secuencia
                  </td>
                  <td>:</td>
                  <td>
                    <ul>
                      <xsl:for-each select="Eventos/Secuencia">
                        <li>
                          <xsl:value-of select="Item"/>
                        </li>
                      </xsl:for-each>
                    </ul>
                  </td>
                </tr>
              </table>
            </div>

          </td>
        </tr>

      </table>

    </div>
    
  </xsl:template>


</xsl:stylesheet>