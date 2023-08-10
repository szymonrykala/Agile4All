import * as React from 'react';
import Box, { BoxProps } from '@mui/joy/Box';

function Root(props: BoxProps) {
  return (
    <Box
      {...props}
      sx={{
        bgcolor: 'background.appBody',
        display: 'grid',
        gridTemplateColumns: {
          xs: '1fr',
          sm: 'minmax(0px, auto) minmax(400px, 1fr)',
          md: 'minmax(0px, auto) minmax(480px, 1fr)',
        },
        gridTemplateRows: '64px 1fr',
        minHeight: '100vh',
        ...props.sx,
      }}
    />
  );
}

function Header(props: BoxProps) {
  return (
    <Box
      component="header"
      {...props}
      sx={{
        p: 2,
        gap: 2,
        bgcolor: 'background.componentBg',
        display: 'flex',
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
        gridColumn: '1 / -1',
        borderBottom: '1px solid',
        borderColor: 'divider',
        position: 'sticky',
        top: 0,
        zIndex: 1100,
        ...props.sx,
      }}
    />
  );
}

interface ISideNav extends BoxProps {
  open: boolean
}

function SideNav(props: ISideNav) {
  const commonStyle = React.useMemo(() => {
    return {
      width: props.open ? {
        sm: "220px",
        md: "250px"
      } : "0px",
      transform: `translateX(${!props.open ? "-150px" : "0px"})`,
      transition: "0.3s ease-out",
    }
  }, [props.open])

  return (
    <>
      <Box
        component="nav"
        {...props}
        sx={{
          position: 'fixed',
          top: 50,
          height: "100vh",

          p: props.open ? 1 : 1,
          bgcolor: 'background.componentBg',
          borderRight: '1px solid',
          borderColor: 'divider',
          overflow: 'hidden',
          zIndex: 1000,

          ...commonStyle,
          ...props.sx,
        }}
      >
        {props.children}
      </Box>
      <Box sx={commonStyle}></Box>{/* space placeholder */}
    </>
  );
}


function Main(props: BoxProps) {
  return (
    <Box
      component="main"
      className="Main"
      {...props}
      sx={{
        p: 2,
        ...props.sx
      }}
    />
  );
}

const layout = {
  Root,
  Header,
  SideNav,
  Main,
};

export default layout