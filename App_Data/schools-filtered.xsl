<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="3.0">
  <xsl:param name="schooltype" /><!--NEW: add a parameter named schooltype, can use it via $schooltype-->

  <xsl:output method="html" omit-xml-declaration="yes" /><!--NEW: output the template as HTML and omit the <?xml ... ?> in the output-->

  <xsl:template match="/">
    <table>
      <thead>
        <tr>
          <th>School</th>
          <th>Type</th>
          <th>Website</th>
        </tr>
      </thead>
      <tbody>
        <xsl:apply-templates select="//school[type/text()=$schooltype]" /><!--NEW: use the $schooltype parameter in the XPath-->
      </tbody>
    </table>
  </xsl:template>
  <xsl:template match="school">
    <tr>
      <td><xsl:value-of select="name" /></td>
      <td><xsl:value-of select="type[text()!=$schooltype]" /></td><!--NEW: only print the "type" values that don't match the parameter value-->
      <td><xsl:value-of select="url" /></td>
    </tr>
  </xsl:template>
</xsl:stylesheet>