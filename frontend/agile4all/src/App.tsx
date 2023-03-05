import * as React from 'react';
import SideNav from './components/SideNav'
import Layout from './components/Layout';
import Header from './components/Header';
import { Outlet } from 'react-router-dom';
import { useAppSelector } from './hooks';
import { ChatContextProvider } from './components/Chat/Context';
import Chat from './components/Chat';
import ResourceLoader from './components/common/ReourceLoader';


export default function App() {
  const [drawerOpen, setDrawerOpen] = React.useState(false);
  const session = useAppSelector(({ session }) => session);

  return (
    <>
      {session && <ResourceLoader />}
      {drawerOpen && (
        <Layout.SideDrawer onClose={() => setDrawerOpen(false)}>
          <SideNav />
        </Layout.SideDrawer>
      )}

      <Layout.Root
        sx={{
          ...(drawerOpen && {
            height: '100vh',
            overflow: 'hidden',
          }),
        }}
      >
        <ChatContextProvider>
          <Layout.Header >
            <Header
              setDrawerOpen={setDrawerOpen}
              session={session}
            />
          </Layout.Header>

          <Layout.SideNav>
            <SideNav />
          </Layout.SideNav>

          <Layout.Main>
            <Outlet />
            <Chat />
          </Layout.Main>
        </ChatContextProvider>
      </Layout.Root>
    </>
  );
}