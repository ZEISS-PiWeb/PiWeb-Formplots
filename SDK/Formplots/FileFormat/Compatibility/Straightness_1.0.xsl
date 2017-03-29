<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

  <!-- add creator software and version, assume straightness 1.0 data is always created by Caligo -->
  <xsl:template match="/Formplot[@Type='Straightness']">
    <xsl:element name="Formplot">
      <xsl:attribute name="Type">
        <xsl:value-of select="@Type"/>
      </xsl:attribute>
      <xsl:element name="CreatorSoftware">Caligo</xsl:element>
      <xsl:element name="CreatorSoftwareVersion">1.0</xsl:element>
      <xsl:apply-templates select="*"/>
    </xsl:element>
  </xsl:template>

  <!-- create new element "CoordinateSystem" using the old element "ElementSystem", switch attribute values for Axis1 and Axis3 -->
  <xsl:template match="ElementSystem">
    <xsl:element name="CoordinateSystem">
      <xsl:apply-templates select="Origin"/>
      <xsl:element name="Axis1">
        <xsl:attribute name="X">
          <xsl:value-of select="Axis3/@X"/>
        </xsl:attribute>
        <xsl:attribute name="Y">
          <xsl:value-of select="Axis3/@Y"/>
        </xsl:attribute>
        <xsl:attribute name="Z">
          <xsl:value-of select="Axis3/@Z"/>
        </xsl:attribute>
      </xsl:element>
      <xsl:apply-templates select="Axis2"/>
      <xsl:element name="Axis3">
        <xsl:attribute name="X">
          <xsl:value-of select="Axis1/@X"/>
        </xsl:attribute>
        <xsl:attribute name="Y">
          <xsl:value-of select="Axis1/@Y"/>
        </xsl:attribute>
        <xsl:attribute name="Z">
          <xsl:value-of select="Axis1/@Z"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <!-- add default line geometry values before element "Length" -->
  <xsl:template match="Length">
    <xsl:element name="Position">
      <xsl:attribute name="X">0.0</xsl:attribute>
      <xsl:attribute name="Y">0.0</xsl:attribute>
      <xsl:attribute name="Z">0.0</xsl:attribute>
    </xsl:element>
    <xsl:element name="Direction">
      <xsl:attribute name="X">1.0</xsl:attribute>
      <xsl:attribute name="Y">0.0</xsl:attribute>
      <xsl:attribute name="Z">0.0</xsl:attribute>
    </xsl:element>
    <xsl:element name="Deviation">
      <xsl:attribute name="X">0.0</xsl:attribute>
      <xsl:attribute name="Y">0.0</xsl:attribute>
      <xsl:attribute name="Z">1.0</xsl:attribute>
    </xsl:element>
    <xsl:copy>
      <xsl:apply-templates select="@*|node()"/>
    </xsl:copy>
  </xsl:template>

  <!-- identity copying everything as is -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()"/>
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>